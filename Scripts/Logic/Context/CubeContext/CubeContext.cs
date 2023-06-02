using GameFrame;

namespace GXGame
{
    public class CubeConText : Context
    {
        public override void Initialize()
        {
            base.Initialize();
            this.AddSystem<CreateCubeSystem>();
            this.AddSystem<ViewSystem<CubeView>>();
            this.AddSystem<DestroySystem>();
            this.AddSystem<InputSystem>();
            this.AddSystem<InputWorldPosChangeSystem>();
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}