using System;
using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    class BehaviorWorldTest : BehaviorWorld
    {
        public override void Init(BehaviorWorldEntity behaviorWorld)
        {
            base.Init(behaviorWorld);
            behaviorWorld.AddBehavior<BehaviorNo1>();
            behaviorWorld.AddBehavior<BehaviorNo2>();
            
            var data = behaviorWorld.AddData<BehaviorData1>();
            behaviorWorld.ChangeBehavior<BehaviorNo1>(data);
        }
    }

    class BehaviorNo1 : Behavior
    {
        public override void DataJoin(IBehaviorData behaviorData)
        {
            Debug.Log("数据加入No1");
        }
        public override void Update(List<IBehaviorData> behaviorData, float elapseSeconds)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ChangeBehavior<BehaviorNo2>(behaviorData[0]);
            }
        }
        public override void DataLeave(IBehaviorData behaviorData)
        {
            Debug.Log("数据离开No1");
        }
    }

    class BehaviorNo2 : Behavior
    {
        public override void DataJoin(IBehaviorData behaviorData)
        {
            Debug.Log("数据加入No2");
        }
        public override void Update(List<IBehaviorData> behaviorData, float elapseSeconds)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                ChangeBehavior<BehaviorNo1>(behaviorData[0]);
            }
        }
        public override void DataLeave(IBehaviorData behaviorData)
        {
            Debug.Log("数据离开No2");
        }
    }

    class BehaviorData1 : IBehaviorData
    {
        public int Number;

        public string Str;
        public void Dispose()
        {
        }
    }

    public class BehaviorTest
    {
        private BehaviorWorldEntity bwe;
        public void Init(Entity entity)
        {
            bwe = entity.AddChild<BehaviorWorldEntity, Type>(typeof(BehaviorWorldTest));
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            bwe.OnUpdate(elapseSeconds,realElapseSeconds);
        }
    }
}