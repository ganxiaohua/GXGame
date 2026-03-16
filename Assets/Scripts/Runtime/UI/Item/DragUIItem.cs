using System;
using FairyGUI;
using GameFrame.Runtime;
using UnityEngine;
using UnityGXGameFrame.Runtime;

namespace GamePlay.Runtime
{
    public class UIDragData : IDisposable
    {
        public BagData.BagType Type;
        public int Index;
        public Rect Rect;
        public Action<bool> Action;

        public void Dispose()
        {
            Action = null;
        }
    }

    public class DragUIItem : UIItem
    {
        public override void Init(GComponent item, DefaultAssetReference assetReference)
        {
            base.Init(item, assetReference);
            item.draggable = true;
            item.onDragStart.Add(OnDragStart);
        }

        private void OnDragStart(EventContext context)
        {
            context.PreventDefault();
            DragDropManager.inst.StartDrag(null, GLoaderIcon.icon, null, (int) context.data);
            // DragDropManager.inst.dragAgent.Load(PackageName.Common, gLoader.icon, assetReference);
            DragDropManager.inst.dragAgent.size = GLoaderIcon.size;
            DragDropManager.inst.dragAgent.fill = FillType.ScaleFree;
            DragDropManager.inst.dragAgent.pivotAsAnchor = true;
            DragDropManager.inst.dragAgent.onDragEnd.Add(OnDragEnd);
            Visible(false);
        }

        private void OnDragEnd()
        {
            var loader = DragDropManager.inst.dragAgent;
            var data = ReferencePool.Acquire<UIDragData>();
            data.Index = DataIndex;
            data.Type = Type;
            data.Rect = loader.Rect2Ins();
            data.Action = OnDragEndAction;
            EventSend.Instance.FireUIEvent(UIEventMsg.IDragPocketItem, data);
            ReferencePool.Release(data);
            DragDropManager.inst.dragAgent.onDragEnd.Remove(OnDragEnd);
            DragDropManager.inst.Cancel();
        }

        private void OnDragEndAction(bool succ)
        {
            Visible(true);
            if (succ)
            {
                ClearData();
            }
        }
    }
}