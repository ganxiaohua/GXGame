using System;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class Player : ECSComponent
    {
        
    }
    
    public class HP : ECSComponent
    {
        public int Value;
    }
    
    [ViewBind(typeof(IWolrdPosition))]
    public class WorldPos : ECSComponent
    {
        public Vector3 Value;
    }
    
    [ViewBind(typeof(IWorldRotate))]
    public class WorldRotate : ECSComponent
    {
        public Quaternion Value;
    }
    
    [ViewBind(typeof(ILocalPosition))]
    public class LocalPos : ECSComponent
    {
        public Vector3 Value;
    }
    
    [ViewBind(typeof(ILocalRotate))]
    public class LocalRotate : ECSComponent
    {
        public Quaternion Value;
    }
    
    [ViewBind(typeof(ILocalScale))]
    public class LocalScale : ECSComponent
    {
        public Vector3 Value;
    }
    
    public class ViewType : ECSComponent
    {
        public Type Value;
    }


    public class AssetPath : ECSComponent
    {
        public string Value;
    }

    public class DestroyCountdown : ECSComponent
    {
        public float Value;
    }


    public class DirectionSpeed : ECSComponent
    {
        public float Value;
    }
    

    [ViewBind(typeof(IMeshRendererColor))]
    public class MeshRendererColor : ECSComponent
    {
        public Color Value;
    }

    public class UseShareMaterial : ECSComponent
    {
    }

    public class FaceDirection : ECSComponent
    {
        public Vector3 Value;
    }

    public class GXInput : ECSComponent
    {
        
    }

    public class MoveDirection : ECSComponent
    {
        public Vector3 Value;
    }

    public class MoveSpeed : ECSComponent
    {
        public float Value;
    }

    public class TargetPos : ECSComponent
    {
        public Vector2 Value;
    }
    
    public class Monster : ECSComponent
    {
        
    }

    public class CampComponent : ECSComponent
    {
        public Camp Value;
    }
    
    public class UnitTypeComponent : ECSComponent
    {
        public UnitTypeEnum Value;
    }
}