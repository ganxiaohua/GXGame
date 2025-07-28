using GameFrame.Runtime;

namespace GXGame
{
    public class GoBaseView : GameObjectView
    {
        public override void Link(EffEntity effEntity)
        {
            base.Link(effEntity);
            Load(effEntity.GetAssetPath().Value).Forget();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}