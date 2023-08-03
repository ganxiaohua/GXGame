using System;
using GameFrame;
using UnityEngine;
using UnityEngine.Serialization;

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
        public Quaternion Rotate;
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

    [ViewBind]
    public class Direction : ECSComponent
    {
        public Vector3 Dir;
    }
    
    public class DirectionSpeed : ECSComponent
    {
        public float Speed;
    }


    public class InputDirection : ECSComponent
    {
    }
    
    
    public class MoveDirection : ECSComponent
    {
        public Vector3 Dir;
    }
    
    public class MoveSpeed : ECSComponent
    {
        public float Speed;
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