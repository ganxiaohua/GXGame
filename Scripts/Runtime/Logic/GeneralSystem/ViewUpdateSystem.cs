using GameFrame;


namespace GXGame
{
    public class ViewUpdateSystem : IInitializeSystem<World>, IUpdateSystem
    {
        private Group viewGroup;

        public void Initialize(World world)
        {
            var matcher = Matcher.SetAll(Components.View);
            viewGroup = world.GetGroup(matcher);
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            ViewGroupUpdate(elapseSeconds, realElapseSeconds);
        }


        private void ViewGroupUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in viewGroup)
            {
                var view = entity.GetView();
                view.Value.OnUpdate(elapseSeconds, realElapseSeconds);
            }
        }

        public void Dispose()
        {
        }
    }
}