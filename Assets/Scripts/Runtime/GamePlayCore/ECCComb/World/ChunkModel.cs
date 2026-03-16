using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using GamePlay.Runtime.MapData;

namespace GamePlay.Runtime
{
    public partial class ChunkModel : IDisposable, IVersions
    {
        private int chunkId;
        private int areaId;
        private JumpIndexArray<EffEntity> mapUnit;
        private MapArea mapArea;
        private DefaultAssetReference defaultAsset;
        private List<HouseModel> houseList;
        private AreaChunkData chunk;

        public int Versions { get; private set; }

        public void Init(MapArea mapArea, int areaId, int chunkId, DefaultAssetReference defaultAsset)
        {
            this.areaId = areaId;
            this.chunkId = chunkId;
            this.defaultAsset = defaultAsset;
            this.mapArea = mapArea;
            houseList = ListPool<HouseModel>.Get(16);
            Versions++;
            LoadChunk().Forget();
        }

        private async UniTask LoadChunk()
        {
            var oldVersions = Versions;
            chunk = await AssetManager.Instance.LoadAsync<AreaChunkData>(string.Format(ConstPath.MapChunkPath, areaId, this.chunkId), defaultAsset);
            if (chunk == null || Versions != oldVersions)
                return;
            mapUnit = new JumpIndexArray<EffEntity>();
            mapUnit.Init(chunk.Units.Count + chunk.ChunkHouseList.Count + chunk.ProtalUnit.Count);
            CreateMapUnit();
            CreateHouse();
            CreateLand();
            ChangePortal();
            // CreateAStarMap();
        }

        public void CreateMapUnit()
        {
            if (chunk == null)
                return;
            if (chunk.Units != null)
            {
                foreach (var t in chunk.Units)
                {
                    if (MapResData.Instance.IsPink(areaId, chunkId, t.Index) ||
                        mapUnit[t.Index] != null)
                        continue;
                    var entity = ConstCreateEntitys.CreateUnitForMapEditor(mapArea.EccWorld, t, mapArea.Map.transform, RemoveUnit);
                    mapUnit.Set(t.Index, entity);
                }
            }
        }


        private void CreateHouse()
        {
            if (chunk.ChunkHouseList != null)
            {
                for (int i = 0; i < chunk.ChunkHouseList.Count; i++)
                {
                    HouseModel houseModel = ReferencePool.Acquire<HouseModel>();
                    houseModel.OnInitialize(chunkId, chunk.ChunkHouseList[i], mapArea);
                    houseList.Add(houseModel);
                }
            }
        }

        private void CreateLand()
        {
            if (chunk.CroplandData != null)
            {
                foreach (var t in chunk.CroplandData)
                {
                    mapArea.AddCropland(t);
                }
            }
        }

        // private void CreateAStarMap()
        // {
        //     if (!chunk.HaveAStarPath)
        //         return;
        // }

        private void RemoveUnit(EffEntity effEntity, ECCWorld world)
        {
            var unitData = effEntity.GetUnitDataComp();
            var data = unitData.GetData();
            mapUnit.Remove(data.MapUnitIndex);
            MapResData.Instance.PickUnit(areaId, chunkId, chunk.Units.Count, data.MapUnitIndex, data.UnitItem.Id);
        }

        public void RefreshDate()
        {
            CreateMapUnit();
            for (int i = 0; i < houseList.Count; i++)
            {
                houseList[i].RefreshDate();
            }
        }


        public void Dispose()
        {
            if (mapUnit != null)
            {
                foreach (var effEntity in mapUnit)
                {
                    if (effEntity.IsAction && !effEntity.HasComponent(ComponentsID<DestroyComp>.TID))
                    {
                        effEntity.AddComponentNoGet<DestroyComp>();
                    }
                }

                mapUnit.Dispose();
                mapUnit = null;
            }

            if (chunk != null)
            {
                if (chunk.CroplandData != null)
                {
                    foreach (var t in chunk.CroplandData)
                    {
                        mapArea.SubCropland(t);
                    }
                }

                chunk = null;
            }

            foreach (var chunk in houseList)
            {
                ReferencePool.Release(chunk);
            }

            ListPool<HouseModel>.Release(houseList);
            Versions++;
            defaultAsset.Dispose();
        }
    }
}