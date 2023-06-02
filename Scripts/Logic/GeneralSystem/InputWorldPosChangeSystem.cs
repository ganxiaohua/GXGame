using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class InputWorldPosChangeSystem : ReactiveSystem
    {
        private Vector3 InputPos;

        public override void Start(Context entity)
        {
            base.Start(entity);
        }

        protected override Collector GetTrigger(Context context) =>
            Collector.CreateCollector(context,Components.InputDirection,Components.WorldPos,Components.InputMoveSpeed);

        protected override bool Filter(ECSEntity entity)
        {
            return entity.GetInputMoveSpeed() != null;
        }

        protected override void Update(List<ECSEntity> entities)
        {
            foreach (var entity in entities)
            {
                float speed = entity.GetInputMoveSpeed().MoveSpeed;
                Vector3 inputDir = entity.GetInputDirection().InputDir*(speed*Time.deltaTime);
                entity.SetWorldOffsetPos(inputDir);
            }
        }

        public override void Clear()
        {
        }
    }
}