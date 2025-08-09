using System.Collections.Generic;
using Common.Runtime;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame
{
    /// <summary>
    /// 由于unity自身碰撞逻辑和渲染并不分离这个组件并不典型.
    /// </summary>
    public class CollisionBox : EffComponent
    {
        public GameObjectProxy Value;

        public static GameObjectProxy Create(EffEntity effEntity, LayerMask layerMask)
        {
            var value = GameObjectProxyPool.Instance.Spawn();
            value.transform.parent = Main.CollisionLayer;
            value.transform.name = effEntity.Name;
            value.gameObject.layer = layerMask;
            var box = value.gameObject.AddComponent<BoxCollider2D>();
            value.gameObject.AddComponent<CollisionEntity>().Entity = effEntity;
            box.size = Vector2.one * 0.5f;
            value.transform.position = effEntity.GetWorldPos().Value;
            return value;
        }

        public override void Dispose()
        {
            var box = Value.gameObject.GetComponent<BoxCollider2D>();
            Object.Destroy(box);
            var entity = Value.gameObject.GetComponent<CollisionEntity>();
            Object.Destroy(entity);
            GameObjectProxyPool.Instance.UnSpawn(Value);
        }
    }

    public class RaycastHit : EffComponent
    {
        public List<RaycastHit2D> Value;
    }


    /// <summary>
    /// 碰到地图碰撞体的行为
    /// </summary>
    public class CollisionGroundType : EffComponent
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