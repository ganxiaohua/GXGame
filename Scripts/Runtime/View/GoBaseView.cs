using GameFrame;

namespace GXGame
{
    public class GoBaseView : GameObjectView
    {
        public override void Link(ECSEntity ecsEntity)
        {
            base.Link(ecsEntity);
            Load(ecsEntity.GetAssetPath().path).Forget();
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}