using System;
using System.Collections.Generic;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame.Runtime.SequenceFrame
{
    public class SequenceFrame : IDisposable
    {
        private int spriteIndex = 0;
        private float interval = 0;
        private float time;
        private ArrayEx<Sprite> sequenceFrame;
        private AsyncLoadAsset<Sprite> sequenceFrameLoader;
        private List<string> spritePaths;

        public void Initialize(List<string> spritePath, float interval = 0.1f)
        {
            time = Time.realtimeSinceStartup;
            this.interval = interval;
            spritePaths = spritePath;
            spriteIndex = 0;
            sequenceFrameLoader = new AsyncLoadAsset<Sprite>(LoadOver);
            sequenceFrameLoader.LoadAssets(spritePaths);
        }

        private void LoadOver(ArrayEx<Sprite> sprites)
        {
            sequenceFrame = sprites;
        }

        public Sprite Do()
        {
            if (sequenceFrame == null)
                return null;
            if ((Time.realtimeSinceStartup - interval) < time)
            {
                var index = Mathf.Max(0, spriteIndex - 1);
                var spirte = sequenceFrame[index];
                return spirte;
            }
            else
            {
                var spirte = sequenceFrame[spriteIndex];
                spriteIndex = (spriteIndex + 1) % spritePaths.Count;
                time = Time.realtimeSinceStartup;
                return spirte;
            }
        }

        public Sprite Stop()
        {
            if (sequenceFrame == null)
                return null;
            spriteIndex = 0;
            var spirte = sequenceFrame[spriteIndex];
            return spirte;
        }

        public void Dispose()
        {
            spriteIndex = 0;
            time = 0;
            sequenceFrameLoader.Clear();
        }
    }
}