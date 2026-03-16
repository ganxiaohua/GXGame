using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;


namespace InTerra
{
    public class InTerra_MaskCreator : EditorWindow
    {
        static bool normalInMask;
        bool info;    
        int format;    
        string path = "";
        string file = "";
        Vector2Int resolution;

        struct TextureField
        {
            public Texture2D texture;
            public int channel;
            public TextureImporter importer;
            public TextureImporterCompression compression;
            public TextureImporterType type;
            public bool isReadable;
            public bool isChanged;
        }

        TextureField metallic;
        TextureField height;
        TextureField ao;
        TextureField smooth;
        TextureField normal;

        bool isResOk;
        bool invertRough;
        float progress;

        public static void OpenWindow(bool normalMask)
        {
            InTerra_MaskCreator window = (InTerra_MaskCreator)EditorWindow.GetWindow<InTerra_MaskCreator>(true, "Mask Map Creator", true);
            Vector2 size = normalMask ? new Vector2(320, 280) : new Vector2(425, 280);
            normalInMask = normalMask;
            window.minSize = size;
            window.maxSize = size;

            Rect main = EditorGUIUtility.GetMainWindowPosition();
            Rect pos = window.position;
            float centerWidth = (main.width - pos.width) * 0.5f;
            float centerHeight = (main.height - pos.height) * 0.5f;
            pos.x = main.x + centerWidth;
            pos.y = main.y + centerHeight;
            window.position = pos;
        }

        void OnGUI()
        {       
            if (normalInMask)
            {
                NormalInMask();
            }
            else
            {
                Mask();
            }

        }

        //============================================================================================
        //------------------------------------    NORMAL-MASK CREATOR   ------------------------------
        //============================================================================================
        void NormalInMask()
        {
            Vector2 size = new Vector2(320, 280);
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new GUILayout.HorizontalScope())
                {
                    using (var check = new EditorGUI.ChangeCheckScope())
                    {
                        normal = SelectTextureField("Normal Map", normal, false);
                        ao = SelectTextureField("A. Occlusion", ao, true);
                        height = SelectTextureField("Heightmap", height, true);

                        if (check.changed)
                        {
                            isResOk = CheckTexturesResolution();
                        }
                    }
                }
            }

            GUI.enabled = false;
            if (normal.texture != null)
            {
                GUI.enabled = true;

                if (!isResOk)
                {
                    EditorGUILayout.HelpBox("Textures must have the same resolution!", MessageType.Warning);
                    size = new Vector2(320, 320);
                    GUI.enabled = false;
                }
            }

            FileAndPathFields(250);

            info = EditorGUILayout.Foldout(info, "Normal-Mask map Texture info", true);
            if (info)
            {
                EditorGUILayout.HelpBox("In the Import Settings the Texture Type has to be set as \"Default\" and \"sRGB(Color Texture)\" has to be unchecked! \n\nChannels info:\nRed - A.Occlusion  \nGreen - Bitangent(Green) from Normal map \nBlue - Height map\nAlpha - Tangent(Red) from Normal map", MessageType.None);
                size = new Vector2(320, 380);
            }

            minSize = size;
            maxSize = size;

            EditorGUILayout.Space();
            GUIStyle buttonStyleBold = new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold };
            if (GUILayout.Button("Save", buttonStyleBold))
            {
                if (!FinalCheck())
                {
                    Focus();
                }
                else
                {

                    EditorUtility.DisplayProgressBar("Saving", "Getting Pixels...", progress);
                    Color[] ga = ImprterSetReadWriteAndGetPixels(ref normal);
                    Color[] r = ImprterSetReadWriteAndGetPixels(ref ao);
                    Color[] b = ImprterSetReadWriteAndGetPixels(ref height);

                    EditorUtility.DisplayProgressBar("Saving", "Setting Pixels...", progress);
                    Color[] outputCol = new Color[ga.Length];

                    if (ao.texture == null) ao.channel = 4;
                    if (height.texture == null) height.channel = 5;

                    for (int i = 0; i < outputCol.Length; i++)
                    {
                        outputCol[i] = new Color(ChooseChanel(r[i], ao.channel), ga[i].g, ChooseChanel(b[i], height.channel), ga[i].r);

                    }

                    Texture2D n_h_ao_texture = new Texture2D(normal.texture.width, normal.texture.height, TextureFormat.RGBAHalf, true);
                    n_h_ao_texture.SetPixels(outputCol);

                    progress += 0.3f;
                    EditorUtility.DisplayProgressBar("Saving", "Encoding and Saving...", progress);

                    SaveMaskTexture(format, n_h_ao_texture, path);
                    DestroyImmediate(n_h_ao_texture);

                    progress += 0.1f;
                    EditorUtility.DisplayProgressBar("Saving", "Importer settings...", progress);

                    ImporterSetOriginalValues(normal);
                    ImporterSetOriginalValues(height);
                    ImporterSetOriginalValues(ao);

                    EditorUtility.ClearProgressBar();

                    EditorApplication.delayCall += () =>
                    {
                        Close();
                    };
                }
            }
        }

        //=======================================================================================
        //-------------------------------    MASK MAP CREATOR   ---------------------------------
        //=======================================================================================
        void Mask()
        {
            Vector2 size = new Vector2(425, 280);

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new GUILayout.HorizontalScope())
                {
                    using (var check = new EditorGUI.ChangeCheckScope())
                    {
                        metallic = SelectTextureField("Metallic", metallic, true);
                        ao = SelectTextureField("A. Occlusion", ao, true);
                        height = SelectTextureField("Heightmap", height, true);
                        using (new GUILayout.VerticalScope())
                        {
                            smooth = SelectTextureField("Smoothness", smooth, true);
                            EditorStyles.label.fontSize = 10;
                            invertRough = EditorGUILayout.ToggleLeft(new GUIContent() { text = "Invert Roughness", tooltip = "This option will convert the Roughness texture into Smoothness texture." }, invertRough);
                            EditorStyles.label.fontSize = 12;
                        }
                        EditorGUILayout.GetControlRect(GUILayout.Width(1));

                        if (check.changed)
                        {
                            isResOk = CheckTexturesResolution();
                        }
                    }
                }
            }

            GUI.enabled = false;
            if (metallic.texture != null || ao.texture != null || height.texture != null || smooth.texture != null)
            {
                GUI.enabled = true;

                if (!isResOk)
                {
                    EditorGUILayout.HelpBox("Textures must have the same resolution!", MessageType.Warning);
                    size = new Vector2(425, 320);
                    GUI.enabled = false;
                }
            }

            minSize = size;
            maxSize = size;

            FileAndPathFields(350);

            EditorGUILayout.Space();
            GUIStyle buttonStyleBold = new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold };
            if (GUILayout.Button("Save", buttonStyleBold))
            {

                if (!FinalCheck())
                {
                    Focus();
                }
                else
                {
                    EditorUtility.DisplayProgressBar("Saving", "Getting Pixels...", progress);
                    Color[] r = ImprterSetReadWriteAndGetPixels(ref metallic);
                    Color[] g = ImprterSetReadWriteAndGetPixels(ref ao);
                    Color[] b = ImprterSetReadWriteAndGetPixels(ref height);
                    Color[] a = ImprterSetReadWriteAndGetPixels(ref smooth);

                    Color[] outputCol = new Color[resolution.x * resolution.y];

                    EditorUtility.DisplayProgressBar("Saving", "Setting Pixels...", progress);

                    if (ao.texture == null) ao.channel = 4;
                    if (height.texture == null) height.channel = 5;

                    for (int i = 0; i < outputCol.Length; i++)
                    {
                        if (invertRough) a[i] = Color.white - a[i];
                        outputCol[i] = new Color(ChooseChanel(r[i], metallic.channel), ChooseChanel(g[i], ao.channel), ChooseChanel(b[i], height.channel), ChooseChanel(a[i], smooth.channel));
                    }

                    progress += 0.1f;
                    EditorUtility.DisplayProgressBar("Saving", "Encoding and Saving...", progress);

                    Texture2D maskTexture = new Texture2D(resolution.x, resolution.y, TextureFormat.RGBAHalf, true);
                    maskTexture.SetPixels(outputCol);

                    SaveMaskTexture(format, maskTexture, path);
                    DestroyImmediate(maskTexture);

                    progress += 0.1f;
                    EditorUtility.DisplayProgressBar("Saving", "Importer settings...", progress);

                    ImporterSetOriginalValues(metallic);
                    ImporterSetOriginalValues(ao);
                    ImporterSetOriginalValues(height);
                    ImporterSetOriginalValues(smooth);

                    EditorUtility.ClearProgressBar();

                    EditorApplication.delayCall += () =>
                    {
                        Close();
                    };
                }
            }
        }


        //=====================================================================================
        GUIContent Tooltip(string tooltip)
        {
            return new GUIContent() { tooltip = tooltip };
        }

        bool CheckTexturesResolution()
        {
            List<int> widthRes = new List<int>();
            List<int> heightRes = new List<int>();

            AddTextureRes(metallic.texture);
            AddTextureRes(ao.texture);
            AddTextureRes(height.texture);
            AddTextureRes(smooth.texture);
            AddTextureRes(normal.texture);

            void AddTextureRes(Texture2D t)
            {
                if (t != null)
                {
                    widthRes.Add(t.width);
                    heightRes.Add(t.width);
                }
            }
            return (widthRes.Distinct().ToList().Count <= 1) && (heightRes.Distinct().ToList().Count <= 1);
        }

        TextureField SelectTextureField(string name, TextureField tf, bool channel)
        {
            GUILayout.BeginVertical();
            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            style.alignment = TextAnchor.UpperCenter;
            style.fixedWidth = 100;
            GUILayout.Label(name, style);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                tf.texture = (Texture2D)EditorGUILayout.ObjectField(tf.texture, typeof(Texture2D), false, GUILayout.Width(100), GUILayout.Height(100));
                if (check.changed)
                {
                    PathAndFile(tf.texture);
                }
            }
            tf.importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(tf.texture)) as TextureImporter;

            if (channel)
            {
                var ch = new List<string> { "Red" };

                if (tf.texture != null && tf.importer != null)
                {
                    if (tf.importer.textureType != TextureImporterType.SingleChannel)
                    {
                        ch.Add("Green");
                        ch.Add("Blue");
                    }

                    if (tf.importer.DoesSourceTextureHaveAlpha())
                    {
                        ch.Add("Alpha");
                    }

                    tf.channel = EditorGUILayout.Popup(Tooltip("Map Channel"), tf.channel, ch.ToArray(), GUILayout.Width(100));
                }
                else
                {
                    GUI.enabled = false;
                    EditorGUILayout.Popup(Tooltip("Map Channel"), tf.channel, ch.ToArray(), GUILayout.Width(100));
                    GUI.enabled = true;
                }
            }
            else
            {
                tf.channel = 0;
                EditorGUILayout.GetControlRect(GUILayout.Width(100));
            }
            GUILayout.EndVertical();

            return tf;
        }

        void PathAndFile(Texture2D texture)
        {
            if (texture != null && string.IsNullOrEmpty(path))
            {
                path = AssetDatabase.GetAssetPath(texture);
                path = Path.GetDirectoryName(path);
                path = path.Replace("\\", "/");
                file = texture.name + "_Mask";
                resolution = new Vector2Int(texture.width, texture.height);
            }
        }

        string FileAndPathFields(int width)
        {
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("File Name:", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("Format:", EditorStyles.miniLabel, GUILayout.MaxWidth(50));
            }
            using (new GUILayout.HorizontalScope())
            {
                string[] sFormat = { "png", "tga" };

                file = EditorGUILayout.TextField(file);
                Repaint();
                format = EditorGUILayout.Popup(format, sFormat, GUILayout.MaxWidth(50));
            }

            EditorGUILayout.LabelField("Path:", EditorStyles.miniLabel);
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.TextField(path, new GUIStyle(EditorStyles.helpBox) { wordWrap = false }, GUILayout.Width(width));

                if (GUILayout.Button("change", EditorStyles.miniButton))
                {
                    string absPath = EditorUtility.SaveFolderPanel("Save textures to folder", path, "");
                    if (absPath.Contains(Application.dataPath))
                    {
                        path = absPath.Substring(absPath.IndexOf("Assets"));
                    }
                    else if (!string.IsNullOrEmpty(absPath))
                    {
                        path = absPath;
                    }
                }
            }
            return path;
        }

        bool FinalCheck()
        {
            if (!CheckTexturesResolution())
            {
                EditorUtility.DisplayDialog("Texture resolution ", "Textures must have the same resolution!", "OK");
            }

            return CheckTexturesResolution() && CheckCrunchedCompression() && ValidPathAndName();
        }

        Color[] ImprterSetReadWriteAndGetPixels(ref TextureField tf)
        {
            if (tf.importer != null)
            {
                tf.isReadable = tf.importer.isReadable;
                tf.compression = tf.importer.textureCompression;
                tf.type = tf.importer.textureType;

                if (!tf.isReadable || tf.compression != TextureImporterCompression.Uncompressed || tf.type != TextureImporterType.Default)
                {
                    tf.importer.isReadable = true;
                    tf.importer.textureCompression = TextureImporterCompression.Uncompressed;
                    tf.importer.crunchedCompression = false;
                    tf.importer.textureType = TextureImporterType.Default;

                    tf.isChanged = true;

                    tf.importer.SaveAndReimport();
                    AssetDatabase.Refresh();
                }

                progress += 0.1f; EditorUtility.DisplayProgressBar("Saving", "Getting Pixels...", progress);
                return tf.texture.GetPixels(0, 0, tf.texture.width, tf.texture.height);
            }
            else
            {
                progress += 0.1f; EditorUtility.DisplayProgressBar("Saving", "Getting Pixels...", progress);
                return new Color[resolution.x * resolution.y]; ;
            }
        }

        bool CheckCrunchedCompression()
        {
            bool isOK = false;
            List<bool> crunched = new List<bool>();

            CheckCrunched(metallic);
            CheckCrunched(ao);
            CheckCrunched(height);
            CheckCrunched(smooth);
            CheckCrunched(normal);

            void CheckCrunched(TextureField tf)
            {
                if (tf.importer != null)
                {
                    crunched.Add(tf.importer.crunchedCompression);
                }
            }

            if (!crunched.Contains(true))
            {
                isOK = true;
            }
            else
            {
                if (EditorUtility.DisplayDialog("Crunched Compression", "Some texture(s) are using Crunched Compression, if you will continue the Crunched Compression will be set off. Do you want to continue?", "Yes", "Cancel"))
                {
                    isOK = true;
                }
            }
            return isOK;
        }


        bool ValidPathAndName()
        {
            bool isOK = false;

            bool nameOk = !string.IsNullOrEmpty(file);
            bool folderOk = path.Length > 3;
            string pf = path;


            if (!folderOk)
            {
                EditorUtility.DisplayDialog("Invalid Folder", "You cannot save the File in the root of the drive, please choose a folder!", "OK");
            }

            if (!nameOk)
            {
                EditorUtility.DisplayDialog("Empty name ", "You need to enter a file name!", "OK");
            }


            if (nameOk && folderOk)
            {
                pf += "/" + file;
                pf += format == 0 ? ".png" : ".tga";
                if (!File.Exists(pf) || (EditorUtility.DisplayDialog("File already exists", "File " + Path.GetFileName(pf) + " already exists. Do you want to replace it?", "Yes", "Cancel")))
                {
                    isOK = true;
                    path = pf;
                }
            }

            return isOK;
        }

        float ChooseChanel(Color col, int channel)
        {
            float output;
            switch (channel)
            {
                case 0:
                    output = col.r;
                    break;
                case 1:
                    output = col.g;
                    break;
                case 2:
                    output = col.b;
                    break;
                case 3:
                    output = col.a;
                    break;
                case 4:
                    output = 1.0f;
                    break;
                case 5:
                    output = 0.5f;
                    break;
                default:
                    return 0.0f;
            }
            return output;
        }

        void SaveMaskTexture(int format, Texture2D texture, string path)
        {
            try
            {
                switch (format)
                {
                    case 0:
                        File.WriteAllBytes(path, texture.EncodeToPNG());
                        break;
                    case 1:
                        File.WriteAllBytes(path, texture.EncodeToTGA());
                        break;
                }
                SetTextureLinear(path);
                AssetDatabase.Refresh();
            }
            catch
            {
                Debug.LogWarning("Saving File " + path + " failed.");
            }
        }

        void SetTextureLinear(string path)
        {
            EditorApplication.delayCall += () =>
            {
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

                if (importer != null)
                {
                    importer.sRGBTexture = false;
                    AssetDatabase.ImportAsset(path);
                    AssetDatabase.Refresh();
                }
            };
        }

        void ImporterSetOriginalValues(TextureField tf)
        {
            if (tf.isChanged)
            {
                tf.importer.isReadable = tf.isReadable;
                tf.importer.textureCompression = tf.compression;
                tf.importer.textureType = tf.type;

                tf.importer.SaveAndReimport();
                AssetDatabase.Refresh();
                progress += 0.1f; EditorUtility.DisplayProgressBar("Saving", "Importer settings...", progress);
            }
        }
    }
}

