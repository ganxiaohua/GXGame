using System.Collections.Generic;
using GameFrame;

namespace GXGame
{
    public class DestroySystem : ReactiveSystem
    {
        public override void Start(World entity)
        {
            base.Start(entity);
        }

        protected override Collector GetTrigger(World world) => Collector.CreateCollector(world, Components.Destroy);

        protected override bool Filter(ECSEntity entity)
        {
            return true;
        }

        protected override void Execute(List<ECSEntity> entities)
        {
            foreach (var item in entities)
            {
                World.RemoveChild(item);
            }
        }

        public override void Clear()
        {
            World = null;
        }
    }
}