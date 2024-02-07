using System.Numerics;
using GameFrame;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace GXGame
{
    public class CreateCubeSystem : IStartSystem<Context>
    {
        public void Start(Context entity)
        {
            var cubeHero = entity.AddChild<Cube>();
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
            cubeHero.AddAssetPath("Cube");
            cubeHero.AddCampComponent(Camp.SELF);
            cubeHero.AddUnitTypeComponent(UnitTypeEnum.HERO);
            
            cubeHero.AddSkillGroupComponent(new int[] {1});
            for (int i = 0; i < 100; i++)
            {
                var monster = entity.AddChild<Cube>();
                monster.AddAssetPath("Cube");
                monster.AddViewType(typeof(CubeView)); 
                monster.AddInputDirection();
                monster.AddMoveSpeed(5);
                monster.AddMoveDirection();
                monster.AddDirectionSpeed(180);
                monster.AddDirection(Vector3.forward);
                // monster.AddSkillGroupComponent(new int[] {1});
                
                monster.AddWorldPos(new Vector3(-6 + (i%10)*1.5f, 0, z: -5+i/10));
                monster.AddWorldRotate(Quaternion.identity);
                monster.AddLocalScale(Vector3.one);
                monster.AddMeshRendererColor(Color.red);
                monster.AddCampComponent(Camp.ENEMY);
                monster.AddUnitTypeComponent(UnitTypeEnum.MONSER);
            }
        }

        public void Clear()
        {
        }
    }
}