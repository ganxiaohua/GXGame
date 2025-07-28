using System;
using System.Collections.Generic;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame
{
    public class ViewBaseSystem : UpdateReactiveSystem
    {
        private Camera camera;

        protected override Collector GetTrigger(World world) => Collector.CreateCollector(world, EcsChangeEventState.ChangeEventState.AddRemoveUpdate,
                ComponentsID<WorldPos>.TID);

        protected override bool Filter(EffEntity entity)
        {
            return entity.HasComponent(ComponentsID<ViewType>.TID);
        }

        protected override void Execute(EffEntity entity)
        {
            bool isInView = IsObjectInView(entity);
            var view = entity.GetView();
            if (isInView && view == null)
            {
                LoadAsset(entity);
            }
            else if (view != null && !isInView)
            {
                entity.RemoveComponent(ComponentsID<View>.TID);
            }
        }

        public override void Dispose()
        {
        }


        private void LoadAsset(EffEntity ecsentity)
        {
            Type type = ecsentity.GetViewType().Value;
            var objectView = View.Create(type);
            objectView.Link(ecsentity);
            ecsentity.AddView(objectView);
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
    }
}