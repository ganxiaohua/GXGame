using System.Collections.Generic;
using GameFrame;

namespace GXGame
{
    public class CubeConText : Context
    {
        public override void Start()
        {
            base.Start();
            this.AddEcsSystem<CreateCubeSystem>();
            this.AddEcsSystem<ViewSystem>();
            this.AddEcsSystem<DestroySystem>();
            this.AddEcsSystem<InputSystem>();
            this.AddEcsSystem<WorldPosChangeSystem>();
            this.AddEcsSystem<WorldDirChangeSystem>();

            //技能相关
            this.AddEcsSystem<CreateSkillManagerSystem>();
            this.AddEcsSystem<SpellStartSystem>();
            this.AddEcsSystem<SkillEffectMoveSystem>();
            this.AddEcsSystem<SkillCollisionSystem>();
        }

        public override void Clear()
        {
            base.Clear();
        }
    }

    public class CubeConText2 : Context
    {
        public override void Start()
        {
            base.Start();
            this.AddEcsSystem<InputSystem>();
            this.AddEcsSystem<WorldPosChangeSystem>();
            this.AddEcsSystem<WorldDirChangeSystem>();
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}