using GamePlay.Runtime;
using GamePlay.Runtime.MapData;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor
{
    public class DrawGrid
    {
        private static readonly int sGap = Shader.PropertyToID("_Gap");
        private static readonly int sStride = Shader.PropertyToID("_Stride");

        public static void DrawGridGizmo(CroplandDataBase gridLayout, Vector3 offset, Color color, ref Mesh gridMesh, ref Material gridMaterial)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            if (gridMesh == null)
                gridMesh = GenerateCachedGridMesh(gridLayout, color);

            if (gridMaterial == null)
            {
                gridMaterial = (Material) EditorGUIUtility.LoadRequired("SceneView/GridGap.mat");
            }

            gridMaterial.SetVector(sGap, gridLayout.CellSize);
            gridMaterial.SetVector(sStride, gridLayout.CellSize);
            gridMaterial.SetPass(0);
            GL.PushMatrix();
            if (gridMesh.GetTopology(0) == MeshTopology.Lines)
                GL.Begin(GL.LINES);
            else
                GL.Begin(GL.QUADS);

            Graphics.DrawMeshNow(gridMesh, gridLayout.Pos + offset, Quaternion.identity);
            GL.End();
            GL.PopMatrix();
        }


        public static Mesh GenerateCachedGridMesh(CroplandDataBase gridLayout, Color color)
        {
            int rows = gridLayout.GirdArea.y;
            int columns = gridLayout.GirdArea.x;
            int horizontalVertices = (rows + 1) * 2;
            int verticalVertices = (columns + 1) * 2;
            Vector3[] vertices = new Vector3[horizontalVertices + verticalVertices];
            int[] indices = new int[vertices.Length];
            Vector2[] uvs = new Vector2[vertices.Length];
            var colors = new Color[vertices.Length];
            int idx = 0;
            var uv0 = new Vector2(50, 0f);
            for (int z = 0; z <= rows; z++)
            {
                vertices[idx++] = new Vector3(0, 0, z * gridLayout.CellSize.y);
                vertices[idx++] = new Vector3(columns * gridLayout.CellSize.x, 0, z * gridLayout.CellSize.y);
            }

            // 生成垂直线顶点
            for (int x = 0; x <= columns; x++)
            {
                vertices[idx++] = new Vector3(x * gridLayout.CellSize.x, 0, 0);
                vertices[idx++] = new Vector3(x * gridLayout.CellSize.x, 0, rows * gridLayout.CellSize.y);
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                indices[i] = i;
                colors[i] = color;
                uvs[i] = uv0;
            }

            var mesh = new Mesh();
            mesh.hideFlags = HideFlags.HideAndDontSave;
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.colors = colors;
            mesh.SetIndices(indices, MeshTopology.Lines, 0);
            return mesh;
        }


        public static void DrawQuadGizmos(CroplandDataBase gridLayout, Vector3 offset, RectInt pos, ref Mesh quadMesh, ref Material quadMaterial, Color color)
        {
            if (Event.current.type != EventType.Repaint)
                return;
            CreateGridMesh(gridLayout, offset, pos, ref quadMesh, ref quadMaterial, color);
        }

        public static void CreateGridMesh(CroplandDataBase gridLayout, Vector3 offset, RectInt rect, ref Mesh quadMesh, ref Material quadMaterial, Color color)
        {
            if (quadMesh == null)
                quadMesh = GenerateQuadMesh(gridLayout, rect);

            if (quadMaterial == null)
            {
                quadMaterial = new Material(Shader.Find("Unlit/Color"));
            }

            quadMaterial.color = color;

            quadMaterial.SetPass(0);
            GL.PushMatrix();
            GL.Begin(GL.QUADS);

            var worldPos = gridLayout.CellCenterToWolrd(rect.position);
            worldPos -= new Vector3(gridLayout.CellSize.x / 2, 0, gridLayout.CellSize.y / 2);
            Graphics.DrawMeshNow(quadMesh, worldPos + offset, Quaternion.identity);
            GL.End();
            GL.PopMatrix();
        }


        private static Mesh GenerateQuadMesh(CroplandDataBase gridLayout, RectInt rect)
        {
            Mesh mesh = new Mesh();
            mesh.hideFlags = HideFlags.HideAndDontSave;

            Vector3[] vertices = new Vector3[4];
            var localPos = new Vector3(0, 0, 0);
            vertices[0] = localPos;
            vertices[1] = localPos + new Vector3(rect.width * gridLayout.CellSize.x, 0, rect.height * gridLayout.CellSize.y); // 1 1
            vertices[2] = localPos + new Vector3(0, 0, rect.height * gridLayout.CellSize.y);                                  // 0 1
            vertices[3] = localPos + new Vector3(rect.width * gridLayout.CellSize.x, 0, 0);                                   //1 0

            int[] triangles = new int[6];
            triangles[0] = 0;
            triangles[1] = 2;
            triangles[2] = 1;
            triangles[3] = 0;
            triangles[4] = 1;
            triangles[5] = 3;

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            return mesh;
        }
    }
}