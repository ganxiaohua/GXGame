using System;
using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public struct OperatedEffectFuncComp : EffComponent
    {
        private int valueIndex;

        public void Init(Func<GameFrame.Runtime.EffEntity, GameFrame.Runtime.EffEntity, bool> data)
        {
            valueIndex = ObjectDatas<Func<GameFrame.Runtime.EffEntity, GameFrame.Runtime.EffEntity, bool>>.Instance.AddData(data);
        }

        public void Set(Func<GameFrame.Runtime.EffEntity, GameFrame.Runtime.EffEntity, bool> data)
        {
            ObjectDatas<Func<GameFrame.Runtime.EffEntity, GameFrame.Runtime.EffEntity, bool>>.Instance.SetData(valueIndex, data);
        }

        public Func<GameFrame.Runtime.EffEntity, GameFrame.Runtime.EffEntity, bool> GetData()
        {
            return ObjectDatas<Func<GameFrame.Runtime.EffEntity, GameFrame.Runtime.EffEntity, bool>>.Instance.GetData(valueIndex);
        }

        public void Dispose()
        {
            ObjectDatas<Func<GameFrame.Runtime.EffEntity, GameFrame.Runtime.EffEntity, bool>>.Instance.RemoveData(valueIndex);
        }
    }

    public struct OperatedFuncComp : EffComponent
    {
        private int valueIndex;

        public void Init(Action<EffEntity, ECCWorld> data)
        {
            valueIndex = ObjectDatas<Action<EffEntity, ECCWorld>>.Instance.AddData(data);
        }


        public void Set(Action<EffEntity, ECCWorld> data)
        {
            ObjectDatas<Action<EffEntity, ECCWorld>>.Instance.SetData(valueIndex, data);
        }

        public Action<EffEntity, ECCWorld> GetData()
        {
            return ObjectDatas<Action<EffEntity, ECCWorld>>.Instance.GetData(valueIndex);
        }

        public void Dispose()
        {
            ObjectDatas<Action<EffEntity, ECCWorld>>.Instance.RemoveData(valueIndex);
        }
    }

    public class BeAttackFuncData
    {
        public Func<EffEntity, ECCWorld, CapabilityBase, float> Start;
        public Action<EffEntity, ECCWorld, CapabilityBase> End;
        public Action<EffEntity, ECCWorld> Death;

        public BeAttackFuncData(Func<EffEntity, ECCWorld, CapabilityBase, float> start, Action<EffEntity, ECCWorld, CapabilityBase> end, Action<EffEntity, ECCWorld> death)
        {
            Start = start;
            End = end;
            Death = death;
        }
    }

    public struct BeAttackFuncComp : EffComponent
    {
        private int valueIndex;

        public void Init(BeAttackFuncData data)
        {
            valueIndex = ObjectDatas<BeAttackFuncData>.Instance.AddData(data);
        }

        public BeAttackFuncData GetData()
        {
            return ObjectDatas<BeAttackFuncData>.Instance.GetData(valueIndex);
        }

        public void Set(BeAttackFuncData data)
        {
            ObjectDatas<BeAttackFuncData>.Instance.SetData(valueIndex, data);
        }

        public void Dispose()
        {
            ObjectDatas<BeAttackFuncData>.Instance.RemoveData(valueIndex);
        }
    }

    public struct BeUseFuncComp : EffComponent
    {
        private int valueIndex;

        public void Init(Action<EffEntity, ECCWorld>[] data)
        {
            valueIndex = ObjectDatas<Action<EffEntity, ECCWorld>[]>.Instance.AddData(data);
        }

        public Action<EffEntity, ECCWorld>[] GetData()
        {
            return ObjectDatas<Action<EffEntity, ECCWorld>[]>.Instance.GetData(valueIndex);
        }

        public void Set(Action<EffEntity, ECCWorld>[] data)
        {
            ObjectDatas<Action<EffEntity, ECCWorld>[]>.Instance.SetData(valueIndex, data);
        }

        public void Dispose()
        {
            ObjectDatas<Action<EffEntity, ECCWorld>[]>.Instance.RemoveData(valueIndex);
        }
    }

    public struct BeOperatedIndex : EffComponent
    {
        public int Value;

        public void Dispose()
        {
        }
    }
}