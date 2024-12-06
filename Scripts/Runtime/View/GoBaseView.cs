using GameFrame;

namespace GXGame
{
    public class GoBaseView : GameObjectView
    {
        public override void Link(ECSEntity ecsEntity)
        {
            base.Link(ecsEntity);
            Load(ecsEntity.GetAssetPath().Value).Forget();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}