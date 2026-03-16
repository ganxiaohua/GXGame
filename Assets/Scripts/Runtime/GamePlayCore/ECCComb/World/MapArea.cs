using System;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using GamePlay.Runtime.MapData;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class MapArea : IVersions, IInitializeSystem<int, ECCWorld>, IDisposable
    {
        private readonly Vector2 showAreaSize = new Vector2(50, 100);

        private readonly Vector2 moveAreaSize = new Vector2(30, 30);

        private readonly Vector2 centerOffset = new Vector2(0, 0);

        public int Versions { get; private set; }

        public int AreaId { get; private set; }

        public GameObjectProxy Map { get; private set; }

        private AreaData areaData;

        private DefaultAssetReference defaultAsset;

        public ECCWorld EccWorld { get; private set; }

        // public AstarPath AstarPath { get; private set; }

        public void OnInitialize(int param, ECCWorld world)
        {
            AreaId = param;
            defaultAsset = new DefaultAssetReference();
            Map = ReferencePool.Acquire<GameObjectProxy>();
            Map.Initialize();
            EccWorld = world;
            InitLandCrop();
        }

        public void Dispose()
        {
            ++Versions;
            DisposeShow();
            DisaposableCropLand();
            defaultAsset.Dispose();
            ReferencePool.Release(Map);
            areaData = null;
        }

        public async UniTask LoadArea()
        {
            var versions = ++Versions;
            var succ = await Map.BindFromAssetAsync(string.Format(ConstPath.MapPath, AreaId), Main.GameObjectLayer);
            if (!succ)
                return;
            // AstarPath = Object.FindFirstObjectByType<AstarPath>();
            var mapDataPath = string.Format(ConstPath.MapDataPath, AreaId);
            areaData = await AssetManager.Instance.LoadAsync<AreaData>(mapDataPath, defaultAsset);
            if (areaData == null)
                return;
            if (versions != Versions)
            {
                defaultAsset.UnrefAsset(mapDataPath);
                return;
            }

            SetChunks();
        }

        public void RefreshDate()
        {
            RefreshClopDate();
            RefreshChunkDate();
        }
    }
}