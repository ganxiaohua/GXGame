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
            Matcher matcher = Matcher.SetAll(Components.DestroyCountdown);
            destroyCountGroup = world.GetGroup(matcher);

            matcher = Matcher.SetAll(Components.AtkIntervalComponent);
            atkIntervalGroup = world.GetGroup(matcher);
            
            matcher = Matcher.SetAll(Components.LateSkillComponent);
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
                var time = entity.GetDestroyCountdown().Time - world.DeltaTime;
                if (time <= 0)
                {
                    entity.AddDestroy();
                    entity.RemoveComponent(Components.DestroyCountdown);
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
                    entity.RemoveComponent(Components.AtkIntervalComponent);
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
                    entity.RemoveComponent(Components.LateSkillComponent);
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