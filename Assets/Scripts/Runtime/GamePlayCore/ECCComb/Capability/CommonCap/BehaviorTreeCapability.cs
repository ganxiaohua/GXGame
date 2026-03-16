using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class BehaviorTreeCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.BeBavior;
        private BehaviourTreeOwner behaviour;
        private Blackboard blackboard;
        private DefaultAssetReference assetReference;


        protected override void OnInit()
        {
            TagList = new List<int>();
            TagList.Add(CapabilityTags.Tag_Behavior);
            assetReference = new DefaultAssetReference();
        }

        public override bool ShouldActivate()
        {
            return Owner.HasComponent(ComponentsID<BehaviorTreeComp>.TID);
        }

        public override bool ShouldDeactivate()
        {
            return !Owner.HasComponent(ComponentsID<BehaviorTreeComp>.TID);
        }

        public override void OnActivated()
        {
            base.OnActivated();
            var gameObject = Owner.GetView().GetData().gameObject;
            behaviour = gameObject.AddComponent<BehaviourTreeOwner>();
            blackboard = gameObject.AddComponent<Blackboard>();
            behaviour.blackboard = blackboard;
            behaviour.updateMode = Graph.UpdateMode.Manual;
            LoadAI().Forget();
        }

        public override void OnDeactivated()
        {
            behaviour.StopBehaviour();
            base.OnDeactivated();
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            behaviour?.UpdateBehaviour();
        }

        private async UniTask LoadAI()
        {
            var behaviorTreeCompPath = Owner.GetBehaviorTreeComp().GetString();
            var obj = await AssetManager.Instance.LoadAsync<BehaviourTree>(behaviorTreeCompPath, assetReference);
            if (obj == null)
                return;
#if UNITY_EDITOR
            obj = UnityEngine.Object.Instantiate(obj);
#endif
            behaviour.graph = obj;
            obj.UpdateReferences(behaviour, blackboard, true);
            behaviour.BindExposedParameters();
            behaviour.StartBehaviour();
        }

        public override void Dispose()
        {
            behaviour.StopBehaviour();
            assetReference.Dispose();
            Object.Destroy(behaviour);
            Object.Destroy(blackboard);
            base.Dispose();
        }
    }
}