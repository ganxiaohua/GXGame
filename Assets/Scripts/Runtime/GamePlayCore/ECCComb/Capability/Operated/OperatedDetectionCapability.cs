using System;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    //操作对象检测
    public class OperatedDetectionCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.OperatedDetectionCapability;
        private Collider[] colliders;
        private ColliderDistanceComparer cdComparer;

        protected override void OnInit()
        {
            colliders = new Collider[16];
            cdComparer = new ColliderDistanceComparer();
        }

        public override bool ShouldActivate()
        {
            return Owner.HasComponent(ComponentsID<CollisionDetectionDataComp>.TID);
        }

        public override bool ShouldDeactivate()
        {
            return true;
        }

        public override void OnActivated()
        {
            base.OnActivated();
            Detection();
        }

        // public override void TickActive(float delatTime, float realElapseSeconds)
        // {
        //     Detection();
        // }

        private void Detection()
        {
            var view = Owner.GetView().GetData();
            var operatedDetectionData = Owner.GetCollisionDetectionDataComp().Value;
            var pos = operatedDetectionData.Pos;
            var rot = operatedDetectionData.Rot;
            var size = operatedDetectionData.Size;
#if UNITY_EDITOR
            DebugSceneView.SetBox(this, pos, rot, size, Color.red, "操作区域");
#endif
            var layer = operatedDetectionData.Layer;
            if (!Owner.HasComponent<OperatedObjectComp>())
            {
                Owner.AddOperatedObjectCompExternal(64);
            }

            var operatedObjectComp = Owner.GetOperatedObjectComp();
            operatedObjectComp.GetData().Clear();
            int overlappingCount = CollisionDetection.OverlapBoxNonAlloc(view.gameObject, colliders,
                    pos,
                    rot,
                    size,
                    layer, 0.01f);
            if (overlappingCount == 0)
            {
                return;
            }

            cdComparer.Origin = pos;
            Array.Sort(colliders, cdComparer);
            Filter(overlappingCount, operatedObjectComp);
            Owner.ReactiveOperatedObjectComp();
        }

        private void Filter(int overlappingCount, OperatedObjectComp operatedObjectComp)
        {
            for (int i = 0; i < overlappingCount; i++)
            {
                var viewBind = colliders[i].GetComponentInChildren<ViewEffBindEnitiy>();
                if (viewBind)
                {
                    var entity = viewBind.Entity;
                    if (!entity.IsAction || entity == Owner)
                        continue;
                    if (Owner.HasComponent<OperatedDetectionFilterComp>())
                    {
                        var operatedDetectionFilterComp = Owner.GetOperatedDetectionFilterComp();
                        //过滤类型
                        var odfc = operatedDetectionFilterComp.Value;
                        if (entity.HasComponent<UnitDataComp>() && (entity.GetUnitDataComp().GetData().Camp == odfc.Camp || entity.ID == odfc.EntityId))
                        {
                            continue;
                        }
                    }

                    operatedObjectComp.GetData().Add(entity);
                }
                else
                {
                    //检测到不可交互物
                    if (!Owner.HasComponent<DestroyComp>())
                        Owner.AddDestroyComp();
                }
            }
        }
    }
}