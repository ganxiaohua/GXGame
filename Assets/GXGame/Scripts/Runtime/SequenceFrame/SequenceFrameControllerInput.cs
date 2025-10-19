using System;
using System.Collections.Generic;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame.Runtime.SequenceFrame
{
    public class SequenceFrameControllerInput : IDisposable
    {
        private SequenceFrameSO sequenceFrameSo;
        private AsyncLoadAsset<SequenceFrameSO> sequenceFrameSoLoader;
        private SequenceFrame walkWithForward;
        private SequenceFrame walkWithRL;
        private SequenceFrame walkWithBelow;

        public void Initialize(string sequenceFramePath)
        {
            sequenceFrameSoLoader = new AsyncLoadAsset<SequenceFrameSO>(LoadOver);
            sequenceFrameSoLoader.LoadAsset(sequenceFramePath);
            walkWithForward = ReferencePool.Acquire<SequenceFrame>();
            walkWithRL = ReferencePool.Acquire<SequenceFrame>();
            walkWithBelow = ReferencePool.Acquire<SequenceFrame>();
        }

        private void LoadOver(ArrayEx<SequenceFrameSO> so)
        {
            sequenceFrameSo = so[0];
            walkWithForward.Initialize(sequenceFrameSo.WalkWithForwardPath);
            walkWithRL.Initialize(sequenceFrameSo.WalkWithRLPath);
            walkWithBelow.Initialize(sequenceFrameSo.WalkWithBelowPath);
        }

        public Sprite Froward()
        {
            return walkWithForward.Do();
        }

        public Sprite RightOrLeft()
        {
            return walkWithRL.Do();
        }

        public Sprite Below()
        {
            return walkWithBelow.Do();
        }

        public void Dispose()
        {
            sequenceFrameSoLoader.Clear();
            ReferencePool.Release(walkWithForward);
            ReferencePool.Release(walkWithRL);
            ReferencePool.Release(walkWithBelow);
            sequenceFrameSo = null;
        }
    }
}