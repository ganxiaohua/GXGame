using System;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame
{
    public class Player : EffComponent
    {
    }

    public class HP : EffComponent
    {
        public int Value;
    }

    [ViewBind(typeof(IWolrdPosition))]
    public class WorldPos : EffComponent
    {
        public Vector3 Value;
    }

    [ViewBind(typeof(IWorldRotate))]
    public class WorldRotate : EffComponent
    {
        public Quaternion Value;
    }

    [ViewBind(typeof(ILocalPosition))]
    public class LocalPos : EffComponent
    {
        public Vector3 Value;
    }

    [ViewBind(typeof(ILocalRotate))]
    public class LocalRotate : EffComponent
    {
        public Quaternion Value;
    }

    [ViewBind(typeof(ILocalScale))]
    public class LocalScale : EffComponent
    {
        public Vector3 Value;
    }

    public class ViewType : EffComponent
    {
        public Type Value;
    }


    public class AssetPath : EffComponent
    {
        public string Value;
    }

    public class DestroyCountdown : EffComponent
    {
        public float Value;
    }


    public class DirectionSpeed : EffComponent
    {
        public float Value;
    }


    // [ViewBind(typeof(IMeshRendererColor))]
    public class MeshRendererColor : EffComponent
    {
        public Color Value;
    }

    public class UseShareMaterial : EffComponent
    {
    }

    [ViewBind(typeof(IFaceDirection))]
    public class FaceDirection : EffComponent
    {
        public Vector3 Value;
    }

    [ViewBind(typeof(IAtkComp))]
    public class AtkStartComp : EffComponent
    {
        public int Value;
    }

    [ViewBind(typeof(IAtkOverComp))]
    public class AtkOverComp : EffComponent
    {
        public int Value;
    }

    public class AtkCompCountdown : EffComponent
    {
        public float Value;
    }

    public class GXInput : EffComponent
    {
    }

    public class MoveDirection : EffComponent
    {
        public Vector3 Value;
    }

    public class MoveSpeed : EffComponent
    {
        public float Value;
    }

    public class TargetPos : EffComponent
    {
        public Vector2 Value;
    }

    public class Monster : EffComponent
    {
    }

    public class CampComponent : EffComponent
    {
        public Camp Value;
    }

    public class UnitTypeComponent : EffComponent
    {
        public UnitTypeEnum Value;
    }
}