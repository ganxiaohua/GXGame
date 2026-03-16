using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using GamePlay.Runtime.MapData;

namespace GamePlay.Runtime
{
    public partial class HouseModel : IVersions, IInitializeSystem<int, ChunkHouseData, MapArea>, IDisposable
    {
        private GameObjectProxy house;

        private DefaultAssetReference defaultAsset;

        private ChunkHouseData chunkHouseData;

        private MapArea mapArea;

        private JumpIndexArray<EffEntity> unitList;

        private HouseData houseData;

        private int chunkId;
        public int Versions { get; private set; }


        public void OnInitialize(int chunkId, ChunkHouseData houseDataId, MapArea world)
        {
            this.chunkId = chunkId;
            chunkHouseData = houseDataId;
            house = ReferencePool.Acquire<GameObjectProxy>();
            house.Initialize();
            this.defaultAsset = new DefaultAssetReference();
            this.mapArea = world;
            Versions++;
            LoadHouse().Forget();
        }

        private async UniTask LoadHouse()
        {
            var versions = ++Versions;
            house.Position = chunkHouseData.lPos;
            house.scale = chunkHouseData.lScale;
            house.Rotation = chunkHouseData.lRot;
            var succ = await house.BindFromAssetAsync(string.Format(ConstPath.HousePath, chunkHouseData.HouseId), Main.GameObjectLayer);
            if (!succ)
                return;
            var mapDataPath = string.Format(ConstPath.HouseAssetPath, chunkHouseData.HouseId);
            houseData = await AssetManager.Instance.LoadAsync<HouseData>(mapDataPath, defaultAsset);
            if (houseData == null)
                return;
            if (versions != Versions)
            {
                defaultAsset.UnrefAsset(mapDataPath);
                return;
            }

            unitList = new JumpIndexArray<EffEntity>();
            unitList.Init(houseData.Units.Count);
            ChangeHouseUnit();
        }

        private void ChangeHouseUnit()
        {
            if (houseData == null)
                return;
            foreach (var t in houseData.Units)
            {
                int index = t.Index;
                if (MapResData.Instance.IsHousePink(mapArea.AreaId, chunkId, chunkHouseData.HouseId, index) ||
                    unitList[index] != null)
                    continue;
                var entity = ConstCreateEntitys.CreateUnitForMapEditor(mapArea.EccWorld, t, house.transform, RemoveUnit);
                unitList.Set(index, entity);
            }
        }

        private void RemoveUnit(EffEntity effEntity, ECCWorld world)
        {
            var unitData = effEntity.GetUnitDataComp();
            unitList.Remove(unitData.GetData().MapUnitIndex);
        }

        public void RefreshDate()
        {
            ChangeHouseUnit();
        }


        public void Dispose()
        {
            if (unitList != null)
            {
                foreach (var effEntity in unitList)
                {
                    if (effEntity.IsAction && !effEntity.HasComponent(ComponentsID<DestroyComp>.TID))
                    {
                        effEntity.AddComponentNoGet<DestroyComp>();
                    }
                }

                unitList.Dispose();
                unitList = null;
            }

            ReferencePool.Release(house);
            defaultAsset?.Dispose();
        }
    }
}