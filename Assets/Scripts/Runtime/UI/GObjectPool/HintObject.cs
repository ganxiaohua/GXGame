using FairyGUI;
using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public class HintObject : GObjectBase
    {
        private GTextField textField;
        private GGraph loaderColor;
        private GTweenCallback tcallBack;

        public override void Initialize(object initData)
        {
            base.Initialize(initData);
            tcallBack = huishouzhi;
        }

        public override void OnLoadOver()
        {
            base.OnLoadOver();
            if (textField == null)
            {
                textField = (GTextField) ((GComponent) Obj).GetChild("title");
                loaderColor = (GGraph) ((GComponent) Obj).GetChild("n5");
                Obj.RemoveFromParent();
                Obj.sortingOrder = ConstOrder.HintObject;
            }

            GRoot.inst.AddChild(Obj);
            var data = (ItemInfo) spawnData;
            textField.text = $"获得{data.Item.Name} X{data.Count}";
            loaderColor.color = ConstData.UIQuaColor[(int) data.Item.Quality];
            Obj.SetXY(GRoot.inst.width / 2 + Obj.width, GRoot.inst.height / 2 + Obj.height);
            DoMove();
        }

        public override void Dispose()
        {
            textField = null;
            loaderColor = null;
            base.Dispose();
        }


        private void DoMove()
        {
            Obj.TweenMoveY(Obj.y - 200, 1.5f).OnComplete(tcallBack).SetEase(EaseType.Linear);
        }

        private void huishouzhi()
        {
            var p = (ObjectPool<HintObject>) Pool;
            p.UnSpawn(this);
            ReferencePool.Release((ItemInfo) spawnData);
        }
    }
}