using System.Numerics;
using GameFrame;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace GXGame
{
    public class CreateCubeSystem : IECSStartSystem
    {
        
        public void Start(Context entity)
        {
            var cubeHero = entity.AddChild<Cube>();
            cubeHero.AddWorldPos(new Vector3(-4, 0, -5));
            cubeHero.AddWorldRotate(Vector3.zero);
            cubeHero.AddLocalScale(Vector3.one * 2);
            cubeHero.AddMeshRendererColor(Color.black);
            cubeHero.AddInputDirection();
            cubeHero.AddInputMoveSpeed(8);
            cubeHero.AddWorldOffsetPos();
            cubeHero.AddAssetPath("Cube");

            // for (int i = 0; i < 5; i++)
            // {
            //     var monster = entity.AddChild<Cube>();
            //     monster.AddWorldPos(new Vector3(-6 + i, 0, -5));
            //     monster.AddWorldRotate(Vector3.zero);
            //     monster.AddLocalScale(Vector3.one);
            //     monster.AddMeshRendererColor(Color.red);
            //     monster.AddAssetPath("Cube");
            // }
        }

        public void Clear()
        {
        }
    }
}