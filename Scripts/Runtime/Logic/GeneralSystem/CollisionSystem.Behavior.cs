using GameFrame;
using UnityEngine;

namespace GXGame
{
    public partial class CollisionSystem
    {
        private void Behavior(RaycastHit2D raycastHit2D,ECSEntity owner)
        {
            var hp = owner.GetHP().Value -1;
            owner.SetHP(hp);
            if (hp == 0)
            {
                owner.AddDestroy();
            }
        }
    }
}