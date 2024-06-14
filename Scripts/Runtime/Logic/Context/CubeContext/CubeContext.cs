using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class CubeConText : World
    {
        public override void Start()
        {
            base.Start();
            this.AddEcsSystem<ViewSystem>();
            this.AddEcsSystem<InputSystem>();
            this.AddEcsSystem<WorldPosChangeSystem>();
            this.AddEcsSystem<WorldDirChangeSystem>();

            //技能相关
            this.AddEcsSystem<CreateSkillManagerSystem>();
            this.AddEcsSystem<SpellStartSystem>();
            this.AddEcsSystem<SkillEffectMoveSystem>();
            this.AddEcsSystem<SkillCollisionSystem>();
            
            //最后执行
            this.AddEcsSystem<DestroySystem>();

            CreateEntity();
        }


        public void CreateEntity()
        {
            var cubeHero = AddChild<Cube>();
            cubeHero.Name = "Hero";
            cubeHero.AddViewType(typeof(CubeView));
            cubeHero.AddWorldPos(new Vector3(1.5f, 0, -5));
            cubeHero.AddWorldRotate(Quaternion.identity);
            cubeHero.AddInputDirection();
            cubeHero.AddLocalScale(Vector3.one * 1.5f);
            cubeHero.AddMeshRendererColor(Color.black);
            cubeHero.AddMoveDirection();
            cubeHero.AddMoveSpeed(10);
            cubeHero.AddDirectionSpeed(360);
            cubeHero.AddDirection(Vector3.forward);
            cubeHero.AddCampComponent(Camp.SELF);
            cubeHero.AddUnitTypeComponent(UnitTypeEnum.HERO);

            cubeHero.AddSkillGroupComponent(new int[] {1});
            for (int i = 0; i < 1000; i++)
            {
                var monster = AddChild<Cube>();
                monster.Name = "redCube";
                monster.AddViewType(typeof(CubeView));
                monster.AddInputDirection();
                monster.AddMoveSpeed(5);
                monster.AddMoveDirection();
                monster.AddDirectionSpeed(180);
                monster.AddDirection(Vector3.forward);
                // monster.AddSkillGroupComponent(new int[] {1});
                monster.AddUseShareMaterial();
                monster.AddWorldPos(new Vector3(-6 + (i % 20) * 1.5f, 0, z: -5 + i / 20));
                monster.AddWorldRotate(Quaternion.identity);
                monster.AddLocalScale(Vector3.one);
                monster.AddMeshRendererColor(Color.red);
                monster.AddCampComponent(Camp.ENEMY);
                monster.AddUnitTypeComponent(UnitTypeEnum.MONSER);
            }
        }

        public override void Clear()
        {
            base.Clear();
        }
    }

    public class CubeConText2 : World
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