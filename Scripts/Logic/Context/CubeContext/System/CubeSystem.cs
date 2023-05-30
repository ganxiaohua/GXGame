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
            cubeHero.AddAssetPath("Cube");
        }

        public void Clear()
        {
        }
    }
}