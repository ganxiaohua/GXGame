using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using GamePlay.Runtime.MapData;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class AreaCropLand : IDisposable, IVersions
    {
        private int croplandId;
        private int areaId;
        private MapArea mapArea;
        private DefaultAssetReference defaultAsset;
        public CroplandData croplandData { get; private set; }

        public int Versions { get; private set; }

        private EffEntity land;

        private Dictionary<int, EffEntity> cropDic;

        public void Init(MapArea mapArea, int areaId, int croplandId, DefaultAssetReference defaultAsset)
        {
            this.areaId = areaId;
            this.croplandId = croplandId;
            this.defaultAsset = defaultAsset;
            this.mapArea = mapArea;
            Versions++;
            cropDic = new();
            LoadCropLand().Forget();
        }

        public void Dispose()
        {
            Versions++;
            defaultAsset.UnrefAsset(string.Format(ConstPath.MapCropLandPath, areaId, this.croplandId));
            foreach (var cropDicValue in cropDic.Values)
            {
                cropDicValue.AddComponentNoGet<DestroyComp>();
            }

            cropDic.Clear();
            croplandData = null;
            if (land != null)
            {
                land.AddComponentNoGet<DestroyComp>();
            }

            land = null;
        }

        private async UniTask LoadCropLand()
        {
            var oldVersions = Versions;
            croplandData = await AssetManager.Instance.LoadAsync<CroplandData>(string.Format(ConstPath.MapCropLandPath, areaId, this.croplandId), defaultAsset);
            if (croplandData == null || Versions != oldVersions)
                return;
            land = ConstCreateEntitys.CreateCropLand(mapArea.EccWorld, this);
            InitCrop();
        }

        public void DaySettlement()
        {
            Grow();
        }

        public void RefreshDate()
        {
            InitWater();
        }
    }
}