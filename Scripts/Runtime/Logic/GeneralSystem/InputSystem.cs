using System.Collections.Generic;
using System.Linq;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class InputSystem : IStartSystem<World>, IUpdateSystem
    {
        private Vector3 InputPos;
        private Group Group;
        private Dictionary<KeyCode, int> keyCode;

        public void Start(World entity)
        {
            Matcher matcher = Matcher.SetAll(Components.MoveDirection, Components.InputDirection).NoneOf(Components.SkillComponent);
            Group = entity.GetGroup(matcher);
            keyCode = new();
            keyCode.Add(KeyCode.A, -1);
            keyCode.Add(KeyCode.D, 1);
            keyCode.Add(KeyCode.W, 1);
            keyCode.Add(KeyCode.S, -1);
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            bool set = false;
            if (InputPos != Vector3.zero)
            {
                set = true;
                InputPos.Set(0, 0, 0);
            }

            int index = 0;
            foreach (var variable in keyCode)
            {
                if (Input.GetKey(variable.Key))
                {
                    set = true;
                    if (index < 2)
                    {
                        InputPos.x = variable.Value;
                    }
                    else
                    {
                        InputPos.y = variable.Value;
                    }
                }

                index++;
            }

            if (!set)
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