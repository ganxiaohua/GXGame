using System.Collections.Generic;
using GameFrame;

namespace GXGame
{
    public class DestroySystem : ReactiveSystem
    {

        protected override Collector GetTrigger(World world) => Collector.CreateCollector(world,EcsChangeEventState.ChangeEventState.AddRemoveUpdate, Components.Destroy);

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

        public override void Dispose()
        {
            World = null;
        }
    }
}