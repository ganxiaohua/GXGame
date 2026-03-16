using UnityEngine;
using UnityEditor;

namespace InTerra
{
	public class InTerra_GUI
	{
		public static void TessellationDistaces(Material targetMat, MaterialEditor editor, ref bool minMax)
		{
			float minDist = targetMat.GetFloat("_TessellationFactorMinDistance");
			float maxDist = targetMat.GetFloat("_TessellationFactorMaxDistance");
			float mipMapLevel = targetMat.GetFloat("_MipMapLevel");
			Vector4 mipMapFade = targetMat.GetVector("_MipMapFade");

			using (new GUILayout.VerticalScope(EditorStyles.helpBox))
			{
				EditorGUILayout.LabelField("Tessellation Factor");

				using (new GUILayout.HorizontalScope())
				{
					minDist = Mathf.Clamp(minDist, mipMapFade.z, mipMapFade.w);
					maxDist = Mathf.Clamp(maxDist, mipMapFade.z, mipMapFade.w);

					EditorGUI.BeginChangeCheck();

					EditorGUILayout.LabelField(minDist.ToString("0.0"), GUILayout.Width(33));
					EditorGUILayout.MinMaxSlider(ref minDist, ref maxDist, mipMapFade.z, mipMapFade.w); //The range is the same as for MipMaps
					EditorGUILayout.LabelField(maxDist.ToString("0.0"), GUILayout.Width(33));

					maxDist = minDist + (float)0.001 >= maxDist ? maxDist + (float)0.001 : maxDist;

					if (EditorGUI.EndChangeCheck())
					{
						editor.RegisterPropertyChangeUndo("Tessellation Factor distance");
						targetMat.SetFloat("_TessellationFactorMinDistance", minDist);
						targetMat.SetFloat("_TessellationFactorMaxDistance", maxDist);

					}
				}
				EditorGUILayout.Space();

				MipMapsFading(targetMat, "Mip Maps", editor, ref minMax);
			}
		}


		public static void MipMapsFading(Material targetMat, string label, MaterialEditor editor, ref bool minMax)
		{
			Vector4 mipMapFade = targetMat.GetVector("_MipMapFade");
			float mipMapLevel = targetMat.GetFloat("_MipMapLevel");

			EditorGUI.BeginChangeCheck();
			using (new GUILayout.HorizontalScope())
			{
				EditorGUILayout.LabelField(label, GUILayout.MinWidth(75));
				EditorGUILayout.LabelField(new GUIContent() { text = "Bias:", tooltip = "Minimal Mip map level where the fading will starts." }, new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleRight }, GUILayout.MaxWidth(62));
				mipMapLevel = EditorGUILayout.IntField((int)mipMapLevel, GUILayout.MaxWidth(25));
			}

			mipMapFade = MinMaxValues(targetMat.GetVector("_MipMapFade"), true, ref minMax);

			if (EditorGUI.EndChangeCheck())
			{
				editor.RegisterPropertyChangeUndo("InTerra Mip Maps Fading");
				targetMat.SetVector("_MipMapFade", mipMapFade);
				targetMat.SetFloat("_MipMapLevel", mipMapLevel);
			}			
		}

		public static Vector4 MinMaxValues(Vector4 intersection, bool distanceRange, ref bool minMax)
		{
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(intersection.x.ToString("0.0"), GUILayout.Width(33));
			EditorGUILayout.MinMaxSlider(ref intersection.x, ref intersection.y, intersection.z, intersection.w);
			EditorGUILayout.LabelField(intersection.y.ToString("0.0"), GUILayout.Width(33));
			GUILayout.EndHorizontal();

			if (distanceRange)
            {
				EditorGUI.indentLevel = 1;
				minMax = EditorGUILayout.Foldout(minMax, "Adjust Distance Range", true);
			}
			else
            {
				EditorGUI.indentLevel = 2;
				minMax = EditorGUILayout.Foldout(minMax, "Adjust Range", true);
			}

			EditorGUI.indentLevel = 0;
			if (minMax)
			{
				GUILayout.BeginHorizontal();

				GUIStyle rightAlignment = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight };
				EditorGUILayout.LabelField("Min:", rightAlignment, GUILayout.Width(45));
				intersection.z = EditorGUILayout.DelayedFloatField(intersection.z, GUILayout.MinWidth(50));

				EditorGUILayout.LabelField("Max:", rightAlignment, GUILayout.Width(45));
				intersection.w = EditorGUILayout.DelayedFloatField(intersection.w, GUILayout.MinWidth(50));

				GUILayout.EndHorizontal();
			}

			intersection.x = Mathf.Clamp(intersection.x, intersection.z, intersection.w);
			intersection.y = Mathf.Clamp(intersection.y, intersection.z, intersection.w);

			intersection.y = intersection.x + (float)0.001 >= intersection.y ? intersection.y + (float)0.001 : intersection.y;

			return intersection;
		}
	}
}

