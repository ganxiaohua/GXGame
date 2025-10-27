using System.Collections.Generic;
using GameFrame.Runtime;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using UnityEngine;

namespace GXGame
{
    public class BehaviorTreeComponent : EffComponent
    {
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                Real();
            }
        }

        private string value;

        private GameObjectProxy gxGameObject;
        private AsyncLoadAsset<Object> asyncLoadAsset;
        private BehaviourTreeOwner behaviourTreeOwner;
        private Blackboard blackboard;

        private void Real()
        {
            if (gxGameObject == null)
            {
                gxGameObject = GameObjectProxyPool.Instance.Spawn();
                gxGameObject.gameObject.name = Owner.Name;
                behaviourTreeOwner = gxGameObject.gameObject.AddComponent<BehaviourTreeOwner>();
                blackboard = behaviourTreeOwner.gameObject.AddComponent<Blackboard>();
                blackboard.AddVariable("Entity", Owner);
                behaviourTreeOwner.blackboard = blackboard;
            }

            asyncLoadAsset ??= new AsyncLoadAsset<Object>(LoadOver);
            asyncLoadAsset.LoadAsset(value);
        }

        private void LoadOver(List<Object> assets)
        {
            gxGameObject = GameObjectProxyPool.Instance.Spawn();
            gxGameObject.gameObject.transform.parent = Main.BTOLayer;
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