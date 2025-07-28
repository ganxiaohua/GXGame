using System.Collections.Generic;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame
{
    public class InputSystem : IInitializeSystem<World>, IUpdateSystem
    {
        private Vector3 inputPos;
        private Group group;
        private Dictionary<KeyCode, int> keyCode;

        public void OnInitialize(World entity)
        {
            Matcher matcher = Matcher.SetAll(
                    ComponentsID<MoveDirection>.TID,
                    ComponentsID<GXInput>.TID,
                    ComponentsID<FaceDirection>.TID).NoneOf(ComponentsID<SkillComponent>.TID);
            group = entity.GetGroup(matcher);
            keyCode = new();
            keyCode.Add(KeyCode.A, -1);
            keyCode.Add(KeyCode.D, 1);
            keyCode.Add(KeyCode.W, 1);
            keyCode.Add(KeyCode.S, -1);
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            bool set = false;
            if (inputPos != Vector3.zero)
            {
                set = true;
                inputPos.Set(0, 0, 0);
            }

            int index = 0;
            foreach (var variable in keyCode)
            {
                if (Input.GetKey(variable.Key))
                {
                    set = true;
                    if (index < 2)
                    {
                        inputPos.x = variable.Value;
                    }
                    else
                    {
                        inputPos.y = variable.Value;
                    }
                }

                index++;
            }

            if (!set)
                return;
            foreach (var entity in group)
            {
                entity.SetFaceDirection(inputPos);
                entity.SetMoveDirection(inputPos);
            }
        }


        public void Dispose()
        {
        }
    }
}