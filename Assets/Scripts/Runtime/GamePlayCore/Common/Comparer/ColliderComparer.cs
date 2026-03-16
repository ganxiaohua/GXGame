using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class ColliderDistanceComparer : IComparer<Collider>
    {
        public Vector3 Origin;

        public int Compare(Collider x, Collider y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;

            float distanceX = Vector3.SqrMagnitude(x.transform.position - Origin);
            float distanceY = Vector3.SqrMagnitude(y.transform.position - Origin);
            return distanceX.CompareTo(distanceY);
        }
    }
}