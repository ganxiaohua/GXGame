using System;
using UnityEngine;

namespace GamePlay.Runtime
{
    public static partial class CollisionDetection
    {
        public static (Vector3 center, float radius, float height, Vector3 bottom, Vector3 top) CalculateCapsuleCollider(
                LogicData capsuleCollider, Vector3 position, Quaternion rotation, float flared)
        {
            Vector3 center = rotation * capsuleCollider.Center + position;
            float radius = capsuleCollider.Radius + flared;
            float height = capsuleCollider.Height + flared * 2;
            Vector3 bottom = center + rotation * Vector3.down * (height / 2 - radius);
            Vector3 top = center + rotation * Vector3.up * (height / 2 - radius);
            return (center, radius, height, bottom, top);
        }

        public static bool CapsuleCastNonAlloc(GameObject self,RaycastHit[] raycastHit, LogicData capsuleCollider, Vector3 pos,
                Quaternion rot,
                Vector3 dir, float dist, out RaycastHit hit,
                int layerMask,
                float skinWidth = 0.01f)
        {
            var ccc = CalculateCapsuleCollider(capsuleCollider, pos, rot, -skinWidth);
            // Array.Clear(raycastHit, 0, raycastHit.Length);
            int count = Physics.CapsuleCastNonAlloc(ccc.top, ccc.bottom, ccc.radius, dir, raycastHit, dist + skinWidth, layerMask,
                    QueryTriggerInteraction.Ignore);
            float directDist = float.MaxValue;
            bool didHit = false;
            hit = default;
            for (int i = 0; i < count; i++)
            {
                var tempHit = raycastHit[i];
                //过滤自己 选出距离最近的
                if (tempHit.collider.gameObject != self && directDist > tempHit.distance)
                {
                    hit = tempHit;
                    directDist = tempHit.distance;
                    didHit = true;
                }
            }

            return didHit;
        }

        public static int OverlapCapsuleNonAlloc(
                GameObject self,
                Collider[] overlapCache,
                LogicData capsuleCollider,
                Vector3 position,
                Quaternion rotation,
                int layerMask,
                QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore,
                float skinWidth = 0.01f)
        {
            var ccc = CalculateCapsuleCollider(capsuleCollider, position, rotation, -skinWidth);
            // Array.Clear(overlapCache, 0, overlapCache.Length);
            int overlap = Physics.OverlapCapsuleNonAlloc(ccc.top, ccc.bottom, ccc.radius, overlapCache, layerMask, queryTriggerInteraction);
            for (int i = 0; i < overlap; i++)
            {
                var tempHit = overlapCache[i];
                //过滤自己
                if (tempHit.gameObject == self)
                {
                    overlapCache[i] = default;
                    overlapCache[i] = overlapCache[overlap - 1];
                    overlap--;
                    break;
                }
            }

            return overlap;
        }
    }
}