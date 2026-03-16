using System.Collections.Generic;
using GamePlay.Runtime;
using GamePlay.Runtime.MapData;
using UnityEngine;

namespace GamePlay.Editor
{
    public partial class GridManager
    {
        private Material buildMaterial;
        private Material buildMaterial2;
        private List<Mesh> barrierMeshList = new List<Mesh>();
        private List<Mesh> findPathMeshList = new List<Mesh>();

        private void BuildPromptMesh(CroplandDataBase gridridData)
        {
            BuildObstacle(gridridData);
            // BuildFindPath(gridridData);
        }

        private void BuildObstacle(CroplandDataBase gridridData)
        {
            int index = 0;
            for (int x = 0; x < gridridData.GirdArea.x; x++)
            {
                for (int y = 0; y < gridridData.GirdArea.y; y++)
                {
                    if (barrierMeshList.Count <= index)
                    {
                        barrierMeshList.Add(null);
                    }

                    Mesh mesh = barrierMeshList[index];
                    DrawGrid.DrawQuadGizmos(gridridData, offset, new RectInt(x, y, 1, 1), ref mesh, ref buildMaterial, Color.gray);
                    barrierMeshList[index] = mesh;
                    index++;
                }
            }
        }


        private void ClearAreaMesh()
        {
            foreach (var item in barrierMeshList)
            {
                DestroyImmediate(item);
            }

            foreach (var item in findPathMeshList)
            {
                DestroyImmediate(item);
            }

            buildMaterial = null;
        }
    }
}