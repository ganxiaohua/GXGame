using System;
using GamePlay.Runtime.MapData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class EditorUnit : MonoBehaviour
    {
        private BaseRST unit;

        private Action<BaseRST, GameObject> destroyAction;

        public virtual void Init(BaseRST unit, Action<BaseRST, GameObject> action)
        {
            this.destroyAction = action;
            this.unit = unit;
        }


        public virtual void OnDrawGizmos()
        {
            if (unit == null)
                return;
            unit.lPos = transform.position;
            unit.lScale = transform.localScale;
            unit.lRot = transform.rotation;
        }

        [Button("删除")]
        private void 删除()
        {
            this.destroyAction?.Invoke(unit, gameObject);
        }
    }
}