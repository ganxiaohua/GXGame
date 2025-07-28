using System.Collections.Generic;
using GameFrame.Runtime;

namespace GXGame
{
    public class DestroyBaseSystem : UpdateReactiveSystem
    {
        protected override Collector GetTrigger(World world) =>
                Collector.CreateCollector(world, EcsChangeEventState.ChangeEventState.AddRemoveUpdate, ComponentsID<Destroy>.TID);

        protected override bool Filter(EffEntity entity)
        {
            return true;
        }

        protected override void Execute(EffEntity Entity)
        {
            World.RemoveChild(Entity);
        }

        public override void Dispose()
        {
            World = null;
        }
    }
}