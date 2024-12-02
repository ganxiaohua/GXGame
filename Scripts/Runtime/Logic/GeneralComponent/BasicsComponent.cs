using System;
using Common.Runtime;
using GameFrame;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GXGame
{
    [ViewBind(typeof(IWolrdPosition))]
    public class WorldPos : ECSComponent
    {
        public Vector3 Pos;
    }

    [ViewBind(typeof(IWorldRotate))]
    public class WorldRotate : ECSComponent
    {
        public Quaternion Rotate;
    }

    [ViewBind(typeof(ILocalPosition))]
    public class LocalPos : ECSComponent
    {
        public Vector3 Pos;
    }

    [ViewBind(typeof(ILocalRotate))]
    public class LocalRotate : ECSComponent
    {
        public Quaternion Rotate;
    }

    [ViewBind(typeof(ILocalScale))]
    public class LocalScale : ECSComponent
    {
        public Vector3 Scale;
    }

    public class AssetPath : ECSComponent
    {
        public string path;
    }
    

    public class DirectionSpeed : ECSComponent
    {
        public float DirSpeed;
    }
    

    [ViewBind(typeof(IMeshRendererColor))]
    public class MeshRendererColor : ECSComponent
    {
        public Color Color;
    }

    public class UseShareMaterial : ECSComponent
    {
    }

    public class FaceDirection : ECSComponent
    {
        public Vector3 Dir;
    }

    public class GXInput : ECSComponent
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