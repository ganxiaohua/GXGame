using System;
using UnityEngine;

namespace GamePlay.Runtime
{
    public static partial class CollisionDetection
    {
        public static int OverlapBoxNonAlloc(
                GameObject self,
                Collider[] colliders,
                Vector3 position,
                Quaternion rotation,
                Vector3 size,
                int layerMask,
                float skinWidth,
                QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            // Array.Clear(colliders, 0, colliders.Length);
            int overlap = Physics.OverlapBoxNonAlloc(position, size / 2 - new Vector3(skinWidth, skinWidth, skinWidth), colliders, rotation, layerMask,
                    queryTriggerInteraction);
            for (int i = 0; i < overlap; i++)
            {
                var tempHit = colliders[i];
                if (tempHit.gameObject == self)
                {
                    colliders[i] = default;
                    colliders[i] = colliders[overlap - 1];
                    overlap--;
                    break;
                }
            }

            return overlap;
        }

        public static bool BoxCastNonAlloc(GameObject self, RaycastHit[] raycastHit,
                Vector3 position,
                Quaternion rotation,
                Vector3 dir,
                float distance,
                Vector3 size, out RaycastHit hit,
                int layerMask,
                float skinWidth,
                QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            size -= new Vector3(skinWidth, skinWidth, skinWidth);
            int count = Physics.BoxCastNonAlloc(
                    position,
                    size / 2,
                    dir, raycastHit, rotation, distance + skinWidth,
                    layerMask,
                    queryTriggerInteraction);
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

        public static bool Raycast(Vector3 source, Vector3 direction, float distance, out RaycastHit stepHit, int layerMask = ~0,
                QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            bool didHit = Physics.Raycast(new Ray(source, direction), out stepHit, distance, layerMask, queryTriggerInteraction);
            return didHit;
        }
    }
}