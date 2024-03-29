﻿using System;
using System.Collections.Generic;
using GameFrame;

namespace GXGame
{
    public class ViewSystem : ReactiveSystem
    {
        public override void Start(Context entity)
        {
            base.Start(entity);
        }

        protected override Collector GetTrigger(Context context) => Collector.CreateCollector(context, Components.AssetPath, Components.ViewType);

        protected override bool Filter(ECSEntity entity)
        {
            if (entity.GetView() == null)
                return true;
            return false;
        }

        protected override void Execute(List<ECSEntity> entities)
        {
            foreach (var entity in entities)
            {
                LoadAsset(entity);
            }
        }

        private void  LoadAsset(ECSEntity ecsentity)
        {
            Type type = ecsentity.GetViewType().Type;
            var objectView = View.Create(type);
            objectView.Link(ecsentity);
            ecsentity.AddView(objectView);
        }

        public override void Clear()
        {

        }
    }
}