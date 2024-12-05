using System.Collections.Generic;
using Common.Runtime;
using GameFrame;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using UnityEngine;

namespace GXGame
{
    public class BehaviorTreeComponent : ECSComponent
    {
        public string BehaviorAsset
        {
            get { return behaviorAsset; }
            set
            {
                behaviorAsset = value;
                Real();
            }
        }

        private string behaviorAsset;

        private GXGameObject gxGameObject;
        private AsyncLoadAsset<Object> asyncLoadAsset;
        private BehaviourTreeOwner behaviourTreeOwner;
        private Blackboard blackboard;
        private void Real()
        {
            if (gxGameObject == null)
            {
                gxGameObject = new GXGameObject();
                gxGameObject.BindFromEmpty();
                gxGameObject.gameObject.name = Owner.Name;
                behaviourTreeOwner = gxGameObject.gameObject.AddComponent<BehaviourTreeOwner>();
                blackboard = behaviourTreeOwner.gameObject.AddComponent<Blackboard>();
                blackboard.AddVariable("Entity", Owner);
                behaviourTreeOwner.blackboard = blackboard;
            }
            asyncLoadAsset ??= new AsyncLoadAsset<Object>(LoadOver);
            asyncLoadAsset.LoadAsset(behaviorAsset);
        }

        private void LoadOver(List<Object> assets)
        {
            gxGameObject.BindFromEmpty(Main.BTOLayer);
            var graph = (BehaviourTree) assets[0];
            behaviourTreeOwner.graph = graph;
            graph.UpdateReferences(behaviourTreeOwner, blackboard, true);
            behaviourTreeOwner.StartBehaviour();
        }

        public override void Dispose()
        {
            gxGameObject.Unbind();
            gxGameObject = null;
            asyncLoadAsset.Clear();
        }
    }
}