using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class LogicView : EffEntityView
    {
        protected DefaultAssetReference reference = new DefaultAssetReference();
        public Collider Collider { get; private set; }

        protected LogicData logicData;

        public override void Initialize(object initData)
        {
            base.Initialize(initData);
            LoadLogic().Forget();
        }

        protected virtual async UniTask LoadLogic()
        {
            var path = BindEntity.GetAssetPathComp().GetData();
            logicData = await AssetManager.Instance.LoadAsync<LogicData>($"{path}_Logic", reference);
            if (logicData == null || State == GameObjectState.Destroy)
                return;
            BindEntity.AddColliderLogicCompExternal(logicData);
            gameObject.layer = logicData.Layer;
            Position += logicData.Pos;
            Rotation *= logicData.Rot;
            scale = new Vector3(scale.x * logicData.Scale.x, scale.y * logicData.Scale.y, scale.z * logicData.Scale.z);
            if (logicData.Type == LogicData.ColliderEnum.None)
                return;
            Collider = null;
            switch (logicData.Type)
            {
                case LogicData.ColliderEnum.BoxCollider:
                {
                    var boxCollider = gameObject.AddComponent<BoxCollider>();
                    boxCollider.center = logicData.Center;
                    boxCollider.size = logicData.Size;
                    Collider = boxCollider;
                    break;
                }
                case LogicData.ColliderEnum.CapsuleCollider:
                {
                    var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
                    capsuleCollider.center = logicData.Center;
                    capsuleCollider.height = logicData.Height;
                    capsuleCollider.direction = logicData.Direction;
                    capsuleCollider.radius = logicData.Radius;
                    Collider = capsuleCollider;
                    break;
                }
                case LogicData.ColliderEnum.SphereCollider:
                {
                    var sphereCollider = gameObject.AddComponent<SphereCollider>();
                    sphereCollider.center = logicData.Center;
                    sphereCollider.radius = logicData.Radius;
                    Collider = sphereCollider;
                    break;
                }
            }
        }


        public override void Dispose()
        {
            reference.UnrefAssets();
            Collider = null;
            base.Dispose();
        }
    }
}