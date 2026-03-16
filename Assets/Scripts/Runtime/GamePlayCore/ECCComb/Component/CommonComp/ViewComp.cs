using System;
using UnityEngine;

namespace GameFrame.Runtime
{
    public struct View : EffComponent
    {
        private int valueIndex;

        public void Init(EffEntityView data)
        {
            valueIndex = ObjectDatas<EffEntityView>.Instance.AddData(data);
        }

        public EffEntityView GetData()
        {
            return ObjectDatas<EffEntityView>.Instance.GetData(valueIndex);
        }

        public void Dispose()
        {
            var data = GetData();
            if (data == null)
                return;
            ReferencePool.Release(data);
            ObjectDatas<EffEntityView>.Instance.RemoveData(valueIndex);
        }


        public static EffEntityView Create(Type type, EffEntity entity, Transform parent, object userData = null)
        {
            var view = (EffEntityView) ReferencePool.Acquire(type);
            var input = ReferencePool.Acquire<EffEntityView.Input>();
            input.Entity = entity;
            input.Parent = parent;
            input.UserData = userData;
            view.Initialize(input);
            return view;
        }
    }
}