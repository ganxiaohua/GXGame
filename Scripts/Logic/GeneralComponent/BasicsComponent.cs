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
    
    public class ViewType : ECSComponent
    {
        public Type Type;
    }

    public class CampComponent : ECSComponent
    {
        public Camp Camp;
    }
    
    public class UnitTypeComponent : ECSComponent
    {
        public UnitTypeEnum UnitTypeEnum;
    }
}