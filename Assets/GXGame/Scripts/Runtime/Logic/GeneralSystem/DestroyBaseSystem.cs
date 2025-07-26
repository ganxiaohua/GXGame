using System.Collections.Generic;
using GameFrame;

namespace GXGame
{
    public class DestroyBaseSystem : UpdateReactiveSystem
    {
        protected override Collector GetTrigger(World world) =>
                Collector.CreateCollector(world, EcsChangeEventState.ChangeEventState.AddRemoveUpdate, ComponentsID<Destroy>.TID);

        protected override bool Filter(ECSEntity entity)
        {
            return true;
        }

        protected override void Execute(ECSEntity Entity)
        {
            World.RemoveChild(Entity);
        }

        public override void Dispose()
        {
            World = null;
        }
    }
}