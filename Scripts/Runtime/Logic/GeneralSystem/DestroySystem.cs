using System.Collections.Generic;
using GameFrame;

namespace GXGame
{
    public class DestroySystem : ReactiveSystem
    {
        public override void Start(Context entity)
        {
            base.Start(entity);
        }

        protected override Collector GetTrigger(Context context) => Collector.CreateCollector(context, Components.Destroy);

        protected override bool Filter(ECSEntity entity)
        {
            return true;
        }

        protected override void Execute(List<ECSEntity> entities)
        {
            foreach (var item in entities)
            {
                Context.RemoveChild(item);
            }
        }

        public override void Clear()
        {
            Context = null;
        }
    }
}