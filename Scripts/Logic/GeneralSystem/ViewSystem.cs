using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame;

namespace GXGame
{
    public class ViewSystem<T> : ReactiveSystem where T : class, IEceView, new()
    {
        private T ObjectView;

        public override void Start(Context entity)
        {
            base.Start(entity);
        }

        protected override Collector GetTrigger(Context context) => Collector.CreateCollector(context, typeof(AssetPath));

        protected override bool Filter(ECSEntity entity)
        {
            if (entity.GetView() == null)
                return true;
            return false;
        }

        protected override void Update(List<ECSEntity> entities)
        {
            foreach (var entity in entities)
            {
                LoadAsset(entity);
            }
        }

        public async UniTask LoadAsset(ECSEntity ecsentity)
        {
            ecsentity.AddView();
            ObjectView = ReferencePool.Acquire<T>();
            ObjectView.Link(ecsentity);
            ecsentity.SetView(ObjectView);
        }

        public override void Clear()
        {
            if (ObjectView != null)
            {
                ReferencePool.Release(ObjectView);
            }
        }
    }
}