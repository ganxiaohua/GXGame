using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class WorldPosChangeSystem : IInitializeSystem<World>, IUpdateSystem
    {
        private Group group;
        private World world;

        public void OnInitialize(World world)
        {
            this.world = world;
            Matcher matcher = Matcher.SetAll(Components.MoveDirection);
            group = world.GetGroup(matcher);
        }


        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            Common();
        }

        private void Common()
        {
            foreach (var entity in group)
            {
                var dir = entity.GetMoveDirection().Value;
                if(dir == Vector3.zero)
                    continue;
                var distance = entity.GetMoveSpeed().Value * world.DeltaTime;
                var pos = entity.GetWorldPos().Value;
                pos += (dir.normalized * distance);
                entity.SetWorldPos(pos);
            }
        }

        public void Dispose()
        {
  
        }
    }
}