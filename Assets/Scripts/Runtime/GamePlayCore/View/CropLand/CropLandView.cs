using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using GamePlay.Runtime.MapData;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class CropLandView : EffEntityView
    {
        private GridData GridData { get; set; }

        private DynamicMesh duDynamicMesh;

        private DynamicMesh waterDynamicMesh;
        public AreaCropLand areaCropLand { get; private set; }

        private CroplandDataBase BaseData { get; set; }

        private Dictionary<int, EffEntity> cropDic;

        private Material mateLnad;
        private Material mateWater;
        private DefaultAssetReference reference;

        public override void Initialize(object initData)
        {
            base.Initialize(initData);
            reference = new DefaultAssetReference();
            GridData = gameObject.AddComponent<GridData>();
            areaCropLand = (AreaCropLand) UserData;
            BaseData = areaCropLand.croplandData.Data;
            GridData.CroplandData = BaseData;
            transform.position = BaseData.lPos;
            transform.localScale = BaseData.lScale;
            transform.rotation = BaseData.lRot;
            LandData.Instance.AddLand(BaseData.Index, BaseData.GirdArea.x * BaseData.GirdArea.y);
            var boxcollider = gameObject.AddComponent<BoxCollider>();
            boxcollider.center = new Vector3(BaseData.GirdArea.x / 2.0f, 0, BaseData.GirdArea.y / 2.0f);
            boxcollider.size = new Vector3(BaseData.GirdArea.x, 1, BaseData.GirdArea.y);
        }

        protected override void OnAfterBind(GameObject go)
        {
            base.OnAfterBind(go);
            LoadMat(go).Forget();
        }

        private async UniTask LoadMat(GameObject go)
        {
            mateLnad = await AssetManager.Instance.LoadAsync<Material>("Assets/Res/_Common/Materials/CropLand", reference);
            if (!mateLnad)
                return;
            duDynamicMesh = new DynamicMesh(BaseData.GirdArea.x, BaseData.GirdArea.y, BaseData.CellSize, mateLnad, go.transform);
            SetLandMesh();

            mateWater = await AssetManager.Instance.LoadAsync<Material>("Assets/Res/_Common/Materials/CropLandWater", reference);
            if (!mateLnad)
                return;
            waterDynamicMesh = new DynamicMesh(BaseData.GirdArea.x, BaseData.GirdArea.y, BaseData.CellSize, mateWater, go.transform);
            SetLandWater();
        }

        public void SetLandMesh()
        {
            if (duDynamicMesh != null)
                duDynamicMesh.GenerateCombinedMesh(LandData.Instance.GetAllFramArray(BaseData.Index).IndexList);
        }

        public void SetLandWater()
        {
            if (waterDynamicMesh != null)
                waterDynamicMesh.GenerateCombinedMesh(LandData.Instance.GetSprinkleWater(BaseData.Index).IndexList);
        }

        public override void Dispose()
        {
            duDynamicMesh?.Dispose();
            duDynamicMesh = null;
            waterDynamicMesh?.Dispose();
            waterDynamicMesh = null;
            reference.Dispose();
            base.Dispose();
        }
    }
}