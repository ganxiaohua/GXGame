using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using Luban;
using UnityEditor;

namespace GamePlay.Runtime
{
    public partial class Tables
    {
        private static Dictionary<string, byte[]> bundles;

        public static async UniTask InitializeAsync()
        {
            var tag = "Excel";
            PackageSearcher.SearchByAssetTag(tag, out var infos);
            bundles = new(infos.Length);
            var bytesList = await AssetManager.Instance.LoadRawsAsync(tag, default);
            for (int i = 0; i < infos.Length; i++)
            {
                bundles[infos[i].Address] = bytesList[i];
            }
        }

        private static Tables instance;

        public static Tables Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Tables(LoadTable);
                    bundles = null;
                }

                return instance;
            }
        }

        private static ByteBuf LoadTable(string path)
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                if (bundles == null)
                    bundles = new();
                if (!bundles.ContainsKey(path))
                {
                    var bytes = File.ReadAllBytes($"Assets/Res/ExcelData/bytes/{path}.bytes");
                    bundles[path] = bytes;
                }
            }
#endif
            return new ByteBuf(bundles[path]);
        }
    }
}