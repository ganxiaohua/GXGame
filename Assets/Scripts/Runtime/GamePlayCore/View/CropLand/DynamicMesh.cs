using System.Collections.Generic;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class DynamicMesh
    {
        private int gridWidth;
        private int gridHeight;
        public Vector2 CellSize;
        private List<Vector3> vertices = new List<Vector3>(1024);
        private List<int> triangles = new List<int>(1356);
        private List<Vector2> uvs = new List<Vector2>(1024);
        private Mesh mesh;
        private UnityGameObjectItem go;

        public DynamicMesh(int gridWidth, int gridHeight, Vector2 cellSize, Material material, Transform parent)
        {
            go = UnitGoPool.Instance.Spawn(parent);
            mesh = new Mesh();
            SetGirdeHeight(gridWidth, gridHeight);
            go.gameObject.AddComponent<MeshFilter>().mesh = mesh;
            go.gameObject.AddComponent<MeshRenderer>().material = material;
            this.CellSize = cellSize;
        }

        public void SetWorldPRS(Vector3 pos, Quaternion qua, Vector3 scale)
        {
            go.gameObject.transform.position = pos;
            go.gameObject.transform.rotation = qua;
            go.gameObject.transform.localScale = scale;
        }

        public void SetGirdeHeight(int gridWidth, int gridHeight)
        {
            this.gridHeight = gridHeight;
            this.gridWidth = gridWidth;
        }

        public void SetAction(bool b)
        {
            go.gameObject.SetActive(b);
        }


        public void GenerateCombinedMesh(List<int> list)
        {
            mesh.Clear();
            if (list.Count == 0)
            {
                return;
            }

            // 存储所有顶点、三角形索引和UV坐标
            vertices.Clear();
            triangles.Clear();
            uvs.Clear();
            foreach (var cellIndex in list)
            {
                int row = cellIndex / gridWidth;
                int col = cellIndex % gridWidth;

                // 计算当前顶点数量的偏移量
                int vertexOffset = vertices.Count;

                // 计算格子在世界坐标中的位置
                Vector3 worldPosition = new Vector3(col * CellSize.x, 0.0001f, row * CellSize.y);

                // 添加四个顶点
                vertices.Add(worldPosition);                                          // 左下角
                vertices.Add(worldPosition + new Vector3(CellSize.x, 0, 0));          // 右下角
                vertices.Add(worldPosition + new Vector3(0, 0, CellSize.y));          // 左上角
                vertices.Add(worldPosition + new Vector3(CellSize.x, 0, CellSize.y)); // 右上角

                // 添加三角形索引（需要加上顶点偏移量）
                triangles.Add(0 + vertexOffset); // 左下角
                triangles.Add(2 + vertexOffset); // 左上角
                triangles.Add(1 + vertexOffset); // 右下角
                triangles.Add(2 + vertexOffset); // 左上角
                triangles.Add(3 + vertexOffset); // 右上角
                triangles.Add(1 + vertexOffset); // 右下角

                // 添加UV坐标
                uvs.Add(new Vector2(0, 0));
                uvs.Add(new Vector2(1, 0));
                uvs.Add(new Vector2(0, 1));
                uvs.Add(new Vector2(1, 1));
            }

            // 设置mesh数据
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();

            // 计算法线
            mesh.RecalculateNormals();
        }

        public void Dispose()
        {
            UnitGoPool.Instance.UnSpawn(go);
        }
    }
}