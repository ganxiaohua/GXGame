using System;
using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class ViewSystem : IStartSystem<World>, IUpdateSystem
    {
        private Group group;
        private Camera camera;
        public void Start(World world)
        {
            Matcher matcher = Matcher.SetAll(Components.ViewType);
            group = world.GetGroup(matcher);

            camera = Camera.main;
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in group)
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

        public void Clear()
        {
            group = null;
        }
    }
}