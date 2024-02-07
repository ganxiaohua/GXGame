using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class InputSystem : IStartSystem<Context>, IUpdateSystem
    {
        private Vector3 InputPos;
        private Group Group;

        public void Start(Context entity)
        {
            Matcher matcher = Matcher.SetAll(Components.MoveDirection).NoneOf(Components.SkillComponent);
            Group = entity.GetGroup(matcher);
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
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

            if (InputPos == Vector3.zero)
                return;
            foreach (var entity in Group)
            {
                entity.SetMoveDirection(InputPos);
            }
        }

        public void Clear()
        {
        }
    }
}