using Common.Runtime;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    /// <summary>
    /// 由于unity自身碰撞逻辑和渲染并不分离这个组件并不典型.
    /// </summary>
    public class CollisionBox : ECSComponent
    {
        public GXGameObject Value;

        public static GXGameObject Create(ECSEntity ecsEntity)
        {
            var value = new GXGameObject();
            value.BindFromEmpty(Main.CollisionLayer);
            value.gameObject.name = ecsEntity.Name;
            var box = value.gameObject.AddComponent<BoxCollider2D>();
            value.gameObject.AddComponent<CollisionEntity>().Entity = ecsEntity;
            box.size = Vector2.one * 0.5f;
            return value;
        }

        public override void Dispose()
        {
            var box = Value.gameObject.GetComponent<BoxCollider2D>();
            Object.Destroy(box);
            var entity = Value.gameObject.GetComponent<CollisionEntity>();
            Object.Destroy(entity);
            Value.Unbind();
            Value = null;
        }
    }

    public class RaycastHit : ECSComponent
    {
        public RaycastHit2D[] RaycastHit2Ds;
        public int Count;
    }


    /// <summary>
    /// 碰到地图碰撞体的行为
    /// </summary>
    public class CollisionGroundType : ECSComponent
    {
        
        public int Type = 0;
        /// <summary>
        /// 滑行
        /// </summary>
        public const int Slide = 0;

        /// <summary>
        /// 弹开
        /// </summary>
        public const int Bomb = 1;

        /// <summary>
        /// 反射
        /// </summary>
        public const int Reflect = 2;
    }
}