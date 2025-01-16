using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GXGame.Editor
{
    public class TileMapCollision
    {
        [MenuItem("Assets/Create/GXGame/TileMapCollision", priority = 1)]
        public static void CreateMapCollisionData()
        {
            GameObject selectedGameObject = Selection.activeGameObject;
            Tilemap tileMap = selectedGameObject.GetComponentInChildren<Tilemap>();
            var tiles = tileMap.GetTilesBlock(new BoundsInt(tileMap.origin, tileMap.size));
            Debug.LogWarning(tiles.Length + "  " + tileMap.origin + "  " + tileMap.size);
            foreach (var t in tiles)
            {
                Debug.Log(t);
            }
        }
    }
}