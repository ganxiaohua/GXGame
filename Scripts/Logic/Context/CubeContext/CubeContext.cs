using GameFrame;

namespace GXGame
{
    public class CubeConText : Context
    {
        public override void Initialize()
        {
            base.Initialize();
            this.AddSystem<CreateCubeSystem>();
            this.AddSystem<ViewSystem>();
            this.AddSystem<DestroySystem>();
            this.AddSystem<InputSystem>();
            this.AddSystem<InputWorldPosChangeSystem>();
            
            //技能相关
            this.AddSystem<CreateSkillManagerSystem>();
            this.AddSystem<SpellStartSystem>();
            this.AddSystem<SkillEffectCreateSystem>();
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}