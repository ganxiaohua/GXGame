using System.Collections.Generic;
using GameFrame;

namespace GXGame
{
    public class DestroySystem : ReactiveSystem
    {
        private Context context;

        public override void Start(Context entity)
        {
            context = entity;
            base.Start(entity);
        }

        protected override Collector GetTrigger(Context context) => Collector.CreateCollector(context, typeof(Destroy));

        protected override bool Filter(ECSEntity entity)
        {
            return true;
        }

        protected override void Update(List<ECSEntity> entities)
        {
            foreach (var item in entities)
            {
                context.RemoveChild(item.ID);
            }
        }

        public override void Clear()
        {
            context = null;
        }
    }
}