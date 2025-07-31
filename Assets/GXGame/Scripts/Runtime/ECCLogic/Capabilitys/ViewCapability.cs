using System;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame.Runtime
{
    public class ViewCapability : CapabilityBase
    {
        private Camera camera;

        public override void Init(SHWorld world, EffEntity owner, int id)
        {
            base.Init(world, owner, id);
        }


        public override bool ShouldActivate()
        {
            //如果在视野范围内，且没有view组件
            return IsObjectInView(Owner) && Owner.GetView() == null;
        }

        public override bool ShouldDeactivate()
        {
            //如果在视野范围外，且有view组件
            return !IsObjectInView(Owner) && Owner.GetView() != null;
        }

        public override void OnActivated()
        {
            base.OnActivated();
            Type type = Owner.GetViewType().Value;
            var view = View.Create(type);
            view.Link(Owner);
            Owner.AddView(view);
        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();
            Owner.RemoveComponent(ComponentsID<View>.TID);
        }


        public override void TickActive(float delatTime, float realElapseSeconds)
        {
        }

        private bool IsObjectInView(EffEntity ecsentity)
        {
            var pos = ecsentity.GetWorldPos();
            camera ??= Camera.main;
            Vector3 viewPos = camera.WorldToViewportPoint(pos.Value);
            bool isInView = viewPos.x > 0 && viewPos.x < 1 &&
                            viewPos.y > 0 && viewPos.y < 1 &&
                            viewPos.z > camera.nearClipPlane && viewPos.z < camera.farClipPlane;
            return isInView;
        }

        public override void Dispose()
        {
        }
    }
}