using System.Numerics;
using GameFrame;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace GXGame
{
    public class CreateCubeSystem : IECSStartSystem
    {
        public void Start(Context entity)
        {
            var cubeHero = entity.AddChild<Cube>();
            cubeHero.AddViewType(typeof(CubeView));
            cubeHero.AddWorldPos(new Vector3(-4, 0, -5));
            cubeHero.AddWorldRotate(Quaternion.identity);
            cubeHero.AddInputDirection();
            cubeHero.AddLocalScale(Vector3.one * 2);
            cubeHero.AddMeshRendererColor(Color.black);
            cubeHero.AddMoveDirection();
            cubeHero.AddMoveSpeed(8);
            cubeHero.AddDirectionSpeed(180);
            cubeHero.AddDirection(Vector3.forward);
            cubeHero.AddAssetPath("Cube");
            cubeHero.AddCampComponent(Camp.SELF);
            cubeHero.AddUnitTypeComponent(UnitTypeEnum.HERO);
            
            cubeHero.AddSkillGroupComponent(new int[] {1});
            for (int i = 0; i < 5; i++)
            {
                var monster = entity.AddChild<Cube>();
                // monster.AddInputDirection();
                // monster.AddInputMoveSpeed(5);

                monster.AddWorldPos(new Vector3(-6 + i*1.5f, 0, -5));
                monster.AddWorldRotate(Quaternion.identity);
                monster.AddLocalScale(Vector3.one);
                monster.AddMeshRendererColor(Color.red);
                monster.AddAssetPath("Cube");
                monster.AddViewType(typeof(CubeView)); 
                monster.AddCampComponent(Camp.ENEMY);
                monster.AddUnitTypeComponent(UnitTypeEnum.MONSER);
            }
        }

        public void Clear()
        {
        }
    }
}