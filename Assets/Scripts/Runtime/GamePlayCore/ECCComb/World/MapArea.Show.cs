using System.Collections.Generic;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class MapArea
    {
        private Rect moveAreaRect = new Rect();
        private Rect showAreaRect = new Rect();
        private JumpIndexArray<int> curChunks;
        private List<int> oldChunks;
        private Dictionary<int, ChunkModel> chunks = new();

        private void SetChunks()
        {
            curChunks ??= new JumpIndexArray<int>();
            curChunks.Clear();
            curChunks.Init(areaData.CellSize.x * areaData.CellSize.y);
            oldChunks = new List<int>(areaData.CellSize.x * areaData.CellSize.y);
#if UNITY_EDITOR
            DebugSceneView.Draw += DrawArea;
#endif
        }

        private void DisposeShow()
        {
#if UNITY_EDITOR
            DebugSceneView.Draw -= DrawArea;
#endif
            foreach (var chunksValue in chunks.Values)
            {
                ReferencePool.Release(chunksValue);
            }

            chunks.Clear();
        }

        public void ObservationTarget(Vector3 pos)
        {
            if (areaData == null)
                return;
            var pos2d = pos.XZCoordinateCVector2();
            var targetPos = pos2d - areaData.StartPoint;
            moveAreaRect = new Rect(targetPos - moveAreaSize / 2, moveAreaSize);
            if (showAreaRect.size != Vector2.zero && IsMoveAreaInsideShowArea())
                return;
            showAreaRect = new Rect(targetPos - showAreaSize / 2 - centerOffset, showAreaSize);
            Aging();
            curChunks.Clear();
            int minx = (int) showAreaRect.position.x / areaData.ChunkSize.x;
            int miny = (int) showAreaRect.position.y / areaData.ChunkSize.y;
            minx = Mathf.Max(minx, 0);
            minx = Mathf.Min(minx, areaData.CellSize.x);
            miny = Mathf.Max(miny, 0);
            miny = Mathf.Min(miny, areaData.CellSize.y);
            int maxx = (int) showAreaRect.max.x / areaData.ChunkSize.x;
            int maxy = (int) showAreaRect.max.y / areaData.ChunkSize.y;
            maxx = Mathf.Max(maxx, 0);
            maxx = Mathf.Min(maxx, areaData.CellSize.x);
            maxy = Mathf.Max(maxy, 0);
            maxy = Mathf.Min(maxy, areaData.CellSize.y);
            for (int y = miny; y <= maxy; y++)
            {
                for (int x = minx; x <= maxx; x++)
                {
                    int id = y * areaData.CellSize.x + x;
                    curChunks.Set(id, id);
                    oldChunks.RemoveSwapBack(id);
                }
            }

            CreateChunk();
            RemvoeChunk();
        }

        private void Aging()
        {
            foreach (var id in curChunks)
            {
                oldChunks.Add(id);
            }
        }

        private void CreateChunk()
        {
            foreach (var id in curChunks)
            {
                if (!chunks.TryGetValue(id, out var chunk))
                {
                    chunk = ReferencePool.Acquire<ChunkModel>();
                    chunk.Init(this, AreaId, id, defaultAsset);
                }

                chunks[id] = chunk;
            }
        }

        private void RefreshChunkDate()
        {
            foreach (var chunk in chunks)
            {
                chunk.Value.RefreshDate();
            }
        }

        private void RemvoeChunk()
        {
            foreach (var id in oldChunks)
            {
                if (chunks.TryGetValue(id, out var chunk))
                {
                    chunk.Dispose();
                    chunks.Remove(id);
                }
            }

            oldChunks.Clear();
        }


        private bool IsMoveAreaInsideShowArea()
        {
            return moveAreaRect.xMin >= showAreaRect.xMin &&
                   moveAreaRect.xMax <= showAreaRect.xMax &&
                   moveAreaRect.yMin >= showAreaRect.yMin &&
                   moveAreaRect.yMax <= showAreaRect.yMax;
        }
    }
}