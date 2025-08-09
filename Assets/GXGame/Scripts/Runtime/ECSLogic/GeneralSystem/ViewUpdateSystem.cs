using GameFrame.Runtime;


namespace GXGame
{
    public class ViewUpdateSystem : SimpleEntity, IInitializeSystem<World>, IUpdateSystem
    {
        private Group viewGroup;

        public void OnInitialize(World world)
        {
            var matcher = Matcher.SetAll(ComponentsID<View>.TID);
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
    }
}