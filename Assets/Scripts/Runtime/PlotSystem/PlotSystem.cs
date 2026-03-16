using System;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace GamePlay.Runtime
{
    public class PlotSystem : Singleton<PlotSystem>, IVersions
    {
        private GameObject plotGo;
        private PlayableDirector playableDirector;
        private int curIndex;
        private DefaultAssetReference assetReference;
        public int Versions { get; private set; }

        private Action stopAction;

        public PlotSystem()
        {
            plotGo = new GameObject();
            plotGo.name = "Plot";
            playableDirector = plotGo.AddComponent<PlayableDirector>();
            assetReference = new DefaultAssetReference();
        }

        public void Play(string assetPath, List<object[]> Objects, Action stopAction)
        {
            this.stopAction = stopAction;
            Load(assetPath, Objects).Forget();
        }

        private async UniTask Load(string assetPath, List<object[]> Objects)
        {
            var ver = ++Versions;
            var timelineAsset = await AssetManager.Instance.LoadAsync<TimelineAsset>(assetPath, assetReference);
            if (timelineAsset == null || ver != Versions)
                return;
            playableDirector.playableAsset = timelineAsset;
            int index = 0;
            foreach (var track in timelineAsset.GetOutputTracks())
            {
                if (track is CinemachineTrack)
                {
                    playableDirector.SetGenericBinding(track, (Object) Objects[index][0]);
                    var clips = track.GetClips();
                    int index2 = 1;
                    foreach (var clip in clips)
                    {
                        var exPosed = new ExposedReference<CinemachineVirtualCameraBase>();
                        exPosed.defaultValue = (CinemachineVirtualCameraBase) Objects[index][index2++];
                        ((CinemachineShot) clip.asset).VirtualCamera = exPosed;
                    }
                }
                else if (track is AnimationTack)
                {
                    var clips = track.GetClips();
                    int index2 = 1;
                    foreach (var clip in clips)
                    {
                        ((AnimationAsset) clip.asset).EffEntity = (EffEntity) Objects[index][index2++];
                    }
                }

                index++;
            }

            playableDirector.stopped += OnStoped;
            playableDirector.Play();
        }

        private void OnStoped(PlayableDirector director)
        {
            stopAction?.Invoke();
        }
    }
}