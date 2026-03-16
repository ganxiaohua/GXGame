using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class DetectionLogoCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.DetectionLogo;
        private Collider[] colliders;
        private ColliderDistanceComparer cdComparer;
        private EffEntity logoEntity;
        private DynamicMesh dynamicMesh;
        private Material logoMaterial;
        private DefaultAssetReference reference;
        private List<int> showIndex;
        private UniqueTimer showCountdown;

        protected override void OnInit()
        {
            colliders = new Collider[16];
            cdComparer = new ColliderDistanceComparer();
            logoEntity = null;
            Filter(ComponentsID<ColliderLogicComp>.TID);
            showIndex = new List<int>();
            showIndex.Add(0);
            reference = new DefaultAssetReference();
            showCountdown = new UniqueTimer(ShowLogo);
            LoadMat().Forget();
        }

        public override bool ShouldActivate()
        {
            return Owner.HasComponent(ComponentsID<ColliderLogicComp>.TID);
        }

        public override bool ShouldDeactivate()
        {
            return !Owner.HasComponent(ComponentsID<ColliderLogicComp>.TID);
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            var view = Owner.GetView().GetData();
            var rot = view.Rotation.normalized;
            var pos = view.Position + rot * Vector3.forward;
            var size = Vector3.one;
#if UNITY_EDITOR
            DebugSceneView.SetBox(this, pos, rot, size, Color.green, "检测区域");
#endif
            var layer = ConstLayer.OperatedLayer;
            int overlappingCount = CollisionDetection.OverlapBoxNonAlloc(view.gameObject, colliders,
                    pos,
                    rot,
                    size,
                    layer, 0.01f);
            if (overlappingCount == 0)
            {
                HideLogo();
                return;
            }

            Array.Sort(colliders, cdComparer);

            for (int i = 0; i < overlappingCount; i++)
            {
                var viewBind = colliders[i].GetComponentInChildren<ViewEffBindEnitiy>();
                if (viewBind)
                {
                    var entity = viewBind.Entity;
                    if (!entity.IsAction)
                        continue;
                    var unitData = entity.GetUnitDataComp();
                    if (unitData.GetData().Camp == CampType.CropLand || !entity.IsAction)
                        continue;
                    if (logoEntity != entity)
                    {
                        logoEntity = entity;
                        showCountdown.Schedule(1);
                    }

                    return;
                }
            }

            HideLogo();
        }

        private async UniTask LoadMat()
        {
            logoMaterial = await AssetManager.Instance.LoadAsync<Material>("Assets/Res/_Common/Materials/Select", reference);
            if (logoMaterial == null)
                return;
        }

        private void ShowLogo()
        {
            if (logoEntity is not {IsAction: true} || logoMaterial == null || !logoEntity.HasComponent<ColliderLogicComp>())
                return;

            var view = logoEntity.GetView().GetData();
            var colliderLogicComp = logoEntity.GetColliderLogicComp().GetLogicData();
            var size = new Vector2(colliderLogicComp.Size.x, colliderLogicComp.Size.z);
            dynamicMesh ??= new DynamicMesh(1, 1, size, logoMaterial, Main.GameObjectLayer);
            Vector3 offset = new Vector3(size.x / 2, -0.1f, size.y / 2);
            dynamicMesh.CellSize = size;
            dynamicMesh.GenerateCombinedMesh(showIndex);
            dynamicMesh.SetWorldPRS(view.Position - view.Rotation * offset, view.Rotation, view.scale * 1.2f);
            dynamicMesh.SetAction(true);
            EventSend.Instance.FireUIEvent(UIEventMsg.IShowIntroduce, logoEntity.GetUnit());
        }

        private void HideLogo()
        {
            if (logoEntity == null)
                return;
            showCountdown.Cancel();
            logoEntity = null;
            dynamicMesh?.SetAction(false);
            EventSend.Instance.FireUIEvent(UIEventMsg.IHideIntroduce, null);
        }

        public override void Dispose()
        {
            base.Dispose();
            showCountdown.Cancel();
            showCountdown = null;
            showIndex.Clear();
            reference.Dispose();
            logoMaterial = null;
            logoEntity = null;
            dynamicMesh?.Dispose();
            dynamicMesh = null;
        }
    }
}