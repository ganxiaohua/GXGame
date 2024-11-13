using System;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class ViewSystem : IStartSystem<World>, IUpdateSystem
    {
        private Group viewTypeGroup;
        private Group viewGroup;
        private Camera camera;

        public void Start(World world)
        {
            Matcher matcher = Matcher.SetAll(Components.ViewType);
            viewTypeGroup = world.GetGroup(matcher);

            matcher = Matcher.SetAll(Components.View);
            viewGroup = world.GetGroup(matcher);

            camera = Camera.main;
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            ViewTypeControl();
            ViewGroupUpdate(elapseSeconds, realElapseSeconds);
        }

        private void ViewTypeControl()
        {
            foreach (var entity in viewTypeGroup)
            {
                bool isInView = IsObjectInView(entity);
                var view = entity.GetView();
                if (isInView && view == null)
                {
                    LoadAsset(entity);
                }
                else if (view != null && !isInView)
                {
                    entity.RemoveComponent(Components.View);
                }
            }
        }

        private void ViewGroupUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in viewGroup)
            {
                var view = entity.GetView();
                if (view != null)
                    entity.GetView().Value.OnUpdate(elapseSeconds, realElapseSeconds);
            }
        }

        private void LoadAsset(ECSEntity ecsentity)
        {
            Type type = ecsentity.GetViewType().Type;
            var objectView = View.Create(type);
            objectView.Link(ecsentity);
            ecsentity.AddView(objectView);
        }


        private bool IsObjectInView(ECSEntity ecsentity)
        {
            var pos = ecsentity.GetWorldPos();
            Vector3 viewPos = camera.WorldToViewportPoint(pos.Pos);

            bool isInView = viewPos.x > 0 && viewPos.x < 1 &&
                            viewPos.y > 0 && viewPos.y < 1 &&
                            viewPos.z > camera.nearClipPlane && viewPos.z < camera.farClipPlane;
            return isInView;
        }

        public void Dispose()
        {
            viewTypeGroup = null;
        }
    }
}