using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DTime : MonoBehaviour
{
    public PlayableDirector director;

    public GameObject go;

    // Start is called before the first frame update
    void Start()
    {
        if (director != null)
        {
            // 绑定动画轨道到目标对象
            TimelineAsset timeline = director.playableAsset as TimelineAsset;
            // var x = timeline.GetOutputTracks();

            // director.SetGenericBinding("A_1", gameObject);
            foreach (var track in timeline.GetOutputTracks())
            {
                director.SetGenericBinding(track, go);
                //轨道里面的一块一块的PlayableAsset
                foreach (var VARIABLE in track.GetClips())
                {
                    Debug.Log(VARIABLE.asset);
                    // VARIABLE.asset=
                    //     track.SetGenericBinding(VARIABLE, gameObject);
                }
            }

            director.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}