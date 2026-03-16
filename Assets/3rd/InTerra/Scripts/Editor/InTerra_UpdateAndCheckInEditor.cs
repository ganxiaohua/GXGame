using UnityEditor;

namespace InTerra
{
	public class InTerra_UpdateAndCheckInEditor : UnityEditor.AssetModificationProcessor
	{
		[InitializeOnLoadMethod]
		static void InTerra_InitializeTerrainDataLoading()
		{
			if (!InTerra_Setting.DisableAllAutoUpdates) EditorApplication.update += InTerra_Data.CheckAndUpdate;
		}
	}
}