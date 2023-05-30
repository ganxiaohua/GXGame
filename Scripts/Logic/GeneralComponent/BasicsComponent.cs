using GameFrame;
using UnityEngine;

namespace GXGame
{
    [ViewBind]
    public class WorldPos : IECSComponent
    {
        public Vector3 Pos;

        public void Clear()
        {
        }
    }

    [ViewBind]
    public class WorldRotate : IECSComponent
    {
        public Vector3 Rotate;

        public void Clear()
        {
        }
    }
    
    [ViewBind]
    public class LocalScale : IECSComponent
    {
        public Vector3 Scale;

        public void Clear()
        {
        }
    }
    
    [ViewBind]
    public class MeshRendererColor : IECSComponent
    {
        public Color Color;
        public void Clear()
        {
        }
    }


    public class AssetPath : IECSComponent
    {
        public string Path;

        public void Clear()
        {
            
        }
    }
}