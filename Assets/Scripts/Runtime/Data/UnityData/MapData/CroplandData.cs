using System;
using UnityEngine;

namespace GamePlay.Runtime.MapData
{
    [Serializable]
    public class CroplandDataBase : BaseRST
    {
        [SerializeField]
        public Vector2 CellSize;

        [SerializeField]
        public Vector2Int GirdArea;

        [SerializeField]
        public Vector3 OffsetPos;


        public Vector3 Pos
        {
            get => OffsetPos + lPos;
        }

        public RectInt GetArea()
        {
            RectInt area = new RectInt(0, 0, GirdArea.x, GirdArea.y);
            return area;
        }

        public Vector3 CellToLocal(Vector2Int pos)
        {
            var vector = new Vector3();
            vector.x = pos.x * CellSize.x + CellSize.x / 2;
            vector.y = Pos.y;
            vector.z = pos.y * CellSize.y + CellSize.y / 2;
            return vector;
        }

        public Vector3 CellCenterToWolrd(Vector2Int pos)
        {
            var vector = new Vector3();
            vector.x = pos.x * CellSize.x + Pos.x + CellSize.x / 2;
            vector.y = Pos.y;
            vector.z = pos.y * CellSize.y + this.Pos.z + CellSize.y / 2;
            return vector;
        }

        public Vector3 CellToWolrd(Vector2Int pos)
        {
            var vector = new Vector3();
            vector.x = pos.x * CellSize.x + Pos.x;
            vector.y = Pos.y;
            vector.z = pos.y * CellSize.y + this.Pos.z;
            return vector;
        }


        public bool InArea(Vector2Int pos)
        {
            RectInt area = GetArea();
            return area.Contains(pos);
        }

        public bool InArea(RectInt rect)
        {
            RectInt area = new RectInt(0, 0, GirdArea.x, GirdArea.y);
            if (rect.xMin >= area.xMin && rect.yMin >= area.yMin && rect.xMax <= area.xMax && rect.yMax <= area.yMax)
            {
                return true;
            }

            return false;
        }

        public Vector3 CellToLocalInterpolated(Vector3 pos)
        {
            var vector = new Vector3();
            vector.x = pos.x * CellSize.x;
            vector.y = Pos.y;
            vector.z = pos.z * CellSize.y;
            return vector;
        }

        public Vector2Int LocalToCell(Vector3 pos)
        {
            Vector2Int vector2Int = new Vector2Int();
            vector2Int.x = Mathf.FloorToInt(pos.x / CellSize.x);
            vector2Int.y = Mathf.FloorToInt(pos.z / CellSize.y);
            return vector2Int;
        }

        public Vector2Int WorldToCell(Vector3 pos)
        {
            Vector2Int v = new Vector2Int();
            pos -= this.Pos;
            v.x = Mathf.FloorToInt(pos.x / CellSize.x);
            v.y = Mathf.FloorToInt(pos.z / CellSize.y);
            v.x = Mathf.Min(v.x, GirdArea.x - 1);
            v.x = Mathf.Max(v.x, 0);
            v.y = Mathf.Min(v.y, GirdArea.y - 1);
            v.y = Mathf.Max(v.y, 0);
            return v;
        }

        public Vector3 IndexToWorld(int index)
        {
            int y = index / GirdArea.x;
            int x = index % GirdArea.x;
            return CellCenterToWolrd(new Vector2Int(x, y));
        }

        public int Cell2Index(Vector2Int cellPos)
        {
            return cellPos.y * GirdArea.x + cellPos.x;
        }

        public int World2Index(Vector3 pos)
        {
            var cellPos = WorldToCell(pos);

            return Cell2Index(cellPos);
        }

        public bool IsWorldPosInArea(Vector3 pos)
        {
            var v = WorldToCell(pos);
            return InArea(v);
        }
    }


    [Serializable]
    public class CroplandData : ScriptableObject
    {
        [SerializeField]
        public CroplandDataBase Data;
    }
}