using GameFrame;

namespace GXGame
{
    public class CountDowntSystem : IInitializeSystem<World>, IUpdateSystem
    {
        private Group atkIntervalGroup;
        private Group destroyCountGroup;
        private Group lateSkillGroup;
        private World world;

        public void OnInitialize(World world)
        {
            this.world = world;
            Matcher matcher = Matcher.SetAll(ComponentsID<DestroyCountdown>.TID);
            destroyCountGroup = world.GetGroup(matcher);

            matcher = Matcher.SetAll(ComponentsID<AtkIntervalComponent>.TID);
            atkIntervalGroup = world.GetGroup(matcher);

            matcher = Matcher.SetAll(ComponentsID<LateSkillComponent>.TID);
            lateSkillGroup = world.GetGroup(matcher);
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            DestroyCountDown();
            AtkIntervalCountDown();
            LateSkillCountDown();
        }

        private void DestroyCountDown()
        {
            foreach (var entity in destroyCountGroup)
            {
                var time = entity.GetDestroyCountdown().Value - world.DeltaTime;
                if (time <= 0)
                {
                    entity.AddDestroy();
                    entity.RemoveComponent(ComponentsID<DestroyCountdown>.TID);
                    continue;
                }

                entity.SetDestroyCountdown(time);
            }
        }

        private void AtkIntervalCountDown()
        {
            foreach (var entity in atkIntervalGroup)
            {
                var time = entity.GetAtkIntervalComponent().Time - world.DeltaTime;
                if (time <= 0)
                {
                    entity.RemoveComponent(ComponentsID<AtkIntervalComponent>.TID);
                    continue;
                }

                entity.SetAtkIntervalComponent(time);
            }
        }

        private void LateSkillCountDown()
        {
            foreach (var entity in lateSkillGroup)
            {
                var time = entity.GetLateSkillComponent().Time - world.DeltaTime;
                if (time <= 0)
                {
                    entity.RemoveComponent(ComponentsID<LateSkillComponent>.TID);
                    continue;
                }

                entity.SetLateSkillComponent(time);
            }
        }

        public void Dispose()
        {
        }
    }
}