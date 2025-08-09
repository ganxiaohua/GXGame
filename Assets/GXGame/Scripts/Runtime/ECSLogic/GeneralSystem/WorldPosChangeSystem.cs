using GameFrame.Runtime;
using UnityEngine;

namespace GXGame
{
    public class WorldPosChangeSystem : SimpleEntity, IInitializeSystem<World>, IUpdateSystem
    {
        private Group group;
        private World world;

        public void OnInitialize(World world)
        {
            this.world = world;
            Matcher matcher = Matcher.SetAll(
                    ComponentsID<MoveDirection>.TID,
                    ComponentsID<MoveSpeed>.TID).NoneOf(ComponentsID<CollisionBox>.TID);
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
                var distance = entity.GetMoveSpeed().Value * world.DeltaTime;
                var pos = entity.GetWorldPos().Value;
                pos += (dir.normalized * distance);
                entity.SetWorldPos(pos);
            }
        }
    }
}