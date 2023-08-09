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
            this.AddSystem<WorldPosChangeSystem>();
            this.AddSystem<WorldDirChangeSystem>();
            
            //技能相关
            this.AddSystem<CreateSkillManagerSystem>();
            this.AddSystem<SpellStartSystem>();
            this.AddSystem<SkillEffectCreateSystem>();
            this.AddSystem<SkillEffectMoveSystem>();
            this.AddSystem<SkillCollisionSystem>();
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}