using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame;
using Unity.VisualScripting;

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

        protected override void Update(List<ECSEntity> entities)
        {
            foreach (var entity in entities)
            {
                LoadAsset(entity);
            }
        }

        public void  LoadAsset(ECSEntity ecsentity)
        {
            Type type = ecsentity.GetViewType().Type;
            IEceView ObjectView = (IEceView)ReferencePool.Acquire(type);
            ObjectView.Link(ecsentity);
            ecsentity.AddView(ObjectView);
        }

        public override void Clear()
        {

        }
    }
}