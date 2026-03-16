using System;
using GamePlay.Runtime.MapData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class PortalEditorUnit : EditorUnit
    {
        public PortalUnit PortalUnit;

        // private Action<BaseRST, GameObject> destroyAction;

        public override void Init(BaseRST unit, Action<BaseRST, GameObject> action)
        {
            base.Init(unit, action);
            PortalUnit = (PortalUnit) unit;
        }


        [Button("删除")]
        private void 删除()
        {
            // this.destroyAction?.Invoke(PortalUnit, gameObject);
        }
    }
}