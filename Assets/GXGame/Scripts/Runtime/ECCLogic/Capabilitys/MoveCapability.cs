using System.Collections.Generic;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame.Runtime
{
    public class MoveCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.Move;
        private int[] keyDir = new[] {0, -1, 1};

        public override void Init(int id, SHWorld world, EffEntity owner)
        {
            base.Init(id, world, owner);
            owner.AddLocalScale(Vector3.one);
            owner.AddFaceDirection(Vector2.right);
            owner.AddWorldPos(Vector3.zero);
            owner.AddMoveSpeed(5);
            TagList = new List<int>();
            TagList.Add(CapabilityTags.Tag_Move);
        }

        public override bool ShouldActivate()
        {
            return true;
        }

        public override bool ShouldDeactivate()
        {
            return false;
        }

        public override void OnActivated()
        {
            base.OnActivated();
        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            var pos = Owner.GetWorldPos().Value;
            var speed = Owner.GetMoveSpeed().Value;
            var faceDir = Vector3.zero;
            int indexX = 0;
            int indexY = 0;
            if (Input.GetKey(KeyCode.A))
                indexX = 1;
            else if (Input.GetKey(KeyCode.D))
                indexX = 2;
            if (Input.GetKey(KeyCode.W))
                indexY = 2;
            else if (Input.GetKey(KeyCode.S))
                indexY = 1;
            faceDir = new Vector3(keyDir[indexX], keyDir[indexY]);
            pos += faceDir.normalized * delatTime * speed;
            Owner.SetFaceDirection(faceDir);
            Owner.SetWorldPos(pos);
        }

        public override void Dispose()
        {
        }
    }
}