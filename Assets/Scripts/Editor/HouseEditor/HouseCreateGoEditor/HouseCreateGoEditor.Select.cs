using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Editor.MapEditor
{
    public partial class HouseCreateGoEditor
    {
        [ShowInInspector]
        [PropertyOrder(1)]
        private FolderMatrix folderMatrix = new FolderMatrix();

        private void InitFolder()
        {
            folderMatrix.InitPrefabsFolders(MapGoCreateData.ResMapEditorData, OnFolderMatrix);
        }

        private void ClearFolder()
        {
            folderMatrix?.ClearSelect(MapGoCreateData.ResMapEditorData);
        }

        private void OnFolderMatrix(HashSet<string> data)
        {
            unitMatrix.SetDrawData(data);
        }
    }
}