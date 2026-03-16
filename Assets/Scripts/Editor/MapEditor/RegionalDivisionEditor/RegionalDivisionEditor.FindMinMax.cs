using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlay.Editor.MapEditor
{
    public partial class RegionalDivisionEditor
    {
        private Vector2[] mapPosData;

        private Vector2[] Find()
        {
            if (mapPosData != null)
                return mapPosData;
            List<Vector3> points;
            var landTerrain = GameObject.Find("IslandTerrain");
            var land = landTerrain.GetComponent<Terrain>();
            if (land != null)
                points = GetTerrainVertices(land);
            else
            {
                points = landTerrain.GetComponent<MeshFilter>().sharedMesh.vertices.ToList();
            }

            GetGridBounds(points, out var min, out var max);
            mapPosData = new Vector2[2];
            mapPosData[0] = min;
            mapPosData[1] = max;
            return mapPosData;
        }

        /// <summary>
        /// 获取Terrain的顶点坐标数据
        /// </summary>
        /// <param name="terrain">Terrain对象</param>
        /// <returns>顶点坐标列表</returns>
        private List<Vector3> GetTerrainVertices(Terrain terrain)
        {
            List<Vector3> vertices = new List<Vector3>();

            if (terrain?.terrainData == null)
                return vertices;

            TerrainData terrainData = terrain.terrainData;
            Vector3 terrainPosition = terrain.transform.position;


            int heightmapResolution = terrainData.heightmapResolution;

            Vector3 terrainSize = terrainData.size;


            float[,] heights = terrainData.GetHeights(0, 0, heightmapResolution, heightmapResolution);


            for (int y = 0; y < heightmapResolution; y++)
            {
                for (int x = 0; x < heightmapResolution; x++)
                {
                    float worldX = terrainPosition.x + (float) x / (heightmapResolution - 1) * terrainSize.x;
                    float worldY = terrainPosition.y + heights[y, x] * terrainSize.y;
                    float worldZ = terrainPosition.z + (float) y / (heightmapResolution - 1) * terrainSize.z;

                    vertices.Add(new Vector3(worldX, worldY, worldZ));
                }
            }

            return vertices;
        }

        public void GetGridBounds(List<Vector3> vertices, out Vector2 min, out Vector2 max)
        {
            if (vertices == null || vertices.Count == 0)
            {
                min = Vector2.zero;
                max = Vector2.zero;
                return;
            }

            min = new Vector2(float.MaxValue, float.MaxValue);
            max = new Vector2(float.MinValue, float.MinValue);

            foreach (var vertex in vertices)
            {
                if (vertex.x < min.x)
                    min.x = vertex.x;
                if (vertex.z < min.y)
                    min.y = vertex.z;

                if (vertex.x > max.x)
                    max.x = vertex.x;
                if (vertex.z > max.y)
                    max.y = vertex.z;
            }
        }
    }
}