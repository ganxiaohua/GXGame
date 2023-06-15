using System;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    [ViewBind]
    public class WorldPos : ECSComponent
    {
        public Vector3 Pos;
    }
    
    [ViewBind]
    public class WorldOffsetPos : ECSComponent
    {
        public Vector3 OffsetPos;
    }

    [ViewBind]
    public class WorldRotate : ECSComponent
    {
        public Vector3 Rotate;
    }
    
    [ViewBind]
    public class LocalScale : ECSComponent
    {
        public Vector3 Scale;
    }
    
    [ViewBind]
    public class MeshRendererColor : ECSComponent
    {
        public Color Color;
    }
    
    public class InputDirection : ECSComponent
    {
        public Vector3 InputDir;
    }
    
    public class InputMoveSpeed : ECSComponent
    {
        public float MoveSpeed;
    }

    public class AssetPath : ECSComponent
    {
        public string Path;
    }

    // [AssignBind(typeof(Cube))]
    // public class CubeTest : ECSComponent
    // {
    //     public string testcube;
    // }
}