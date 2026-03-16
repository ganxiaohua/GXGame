using UnityEngine;
using UnityEngine.Playables;

public class PlayableAsset_1 : PlayableAsset
{
    public GameObject GO;
    public string onPlayName;
    public string onpauseName;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        Debug.Log(owner.name);
        var playable = ScriptPlayable<AssetBehaviour_1>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.go = GO;
        behaviour.onPlayName = onPlayName;
        behaviour.onpauseName = onpauseName;
        return playable;
    }
}