using UnityEngine;
using UnityEngine.Playables;

public class AssetBehaviour_1 : PlayableBehaviour
{
    public GameObject go;
    public string onPlayName;
    public string onpauseName;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (go != null)
        {
            go.name = onPlayName;
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (go != null)
        {
            go.name = onpauseName;
        }
    }
}