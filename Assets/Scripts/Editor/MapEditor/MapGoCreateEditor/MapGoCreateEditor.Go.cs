using System.Collections.Generic;
using GameFrame.Runtime;
using GamePlay.Runtime;
using GamePlay.Runtime.MapData;
using UnityEngine;

namespace GamePlay.Editor.MapEditor
{
    public partial class MapGoCreateEditor
    {
        private void SaveChunk()
        {
            if (mapEditor == null)
                return;
            mapEditor.SaveChunk();
        }

        private void ShowOrHideChunkView(AreaChunkData areaChunkData)
        {
            if (!showEditorGosDic.TryGetValue(areaChunkData.Id, out bool show))
            {
                showEditorGosDic.Add(areaChunkData.Id, false);
            }

            showEditorGosDic[areaChunkData.Id] = !showEditorGosDic[areaChunkData.Id];

            if (showEditorGosDic[areaChunkData.Id])
            {
                if (astarPath != null)
                {
                    string str = string.Format(ConstPath.MapAstarAssetPath, mapEditor.AreaData.Id, areaChunkData.Id);
                    byte[] bytes = Pathfinding.Serialization.AstarSerializer.LoadFromFile(str);
                    astarPath.data.DeserializeGraphs(bytes);
                }

                unitCreateEditor.ShowChunkView(areaChunkData);
                houseCreateEditorEditor.ShowChunkView(areaChunkData);
            }
            else
            {
                if (astarPath != null)
                    astarPath.data.ClearGraphs();
                unitCreateEditor.RemoveChunkView(areaChunkData);
                houseCreateEditorEditor.RemoveChunkView(areaChunkData);
            }
        }

        private void ShowGameObjectButton()
        {
            houseCreateEditorEditor.ShowButton(showGameObjectButton);
            unitCreateEditor.ShowButton(showGameObjectButton);
        }

        public void NoSelectHouse()
        {
            houseCreateEditorEditor.NoSelect();
        }

        public void NoSelectUnit()
        {
            unitCreateEditor.NoSelect();
        }
    }
}