using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class InputSystem : ReactiveSystem
    {
        private Vector3 InputPos;

        public override void Start(Context entity)
        {
            base.Start(entity);
        }

        protected override Collector GetTrigger(Context context) => Collector.CreateCollector(context, Components.InputDirection);

        protected override bool Filter(ECSEntity entity)
        {
            return true;
        }

        protected override void Update(List<ECSEntity> entities)
        {
            InputPos.Set(0, 0, 0);
            if (Input.GetKey(KeyCode.W))
            {
                InputPos.z = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                InputPos.z = -1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                InputPos.x = 1;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                InputPos.x = -1;
            }

            foreach (var entity in entities)
            {
                entity.GetInputDirection().InputDir = InputPos;
            }
        }

        public override void Clear()
        {
        }
    }
}