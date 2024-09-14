using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class AnimTest : MonoBehaviour
{
    enum Types : uint

    {
        localPosx = 0,
        localPosy,
        localPosz,
        localRotx,
        localRoty,
        localRotz,
        localRotw,
        localscalex,
        localscaley,
        localscalez,
        Count,
    }

    // Start is called before the first frame update
    public SkinnedMeshRenderer skinA;
    public AnimationClip clip;
    private Dictionary<string, float[]> s_dic = new();

    void Start()
    {
        Matrix4x4[] bindposes = skinA.sharedMesh.bindposes;
        Transform[] verticesPos = skinA.bones;


        var bingings = AnimationUtility.GetCurveBindings(clip);
        foreach (var bing in bingings)
        {
            AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, bing);
            if (!s_dic.TryGetValue(bing.path, out var list))
            {
                list = new float[(int)Types.Count];
                s_dic.Add(bing.path, list);
            }
            
            Debug.Log($"Path: {bing.path}, Property: {bing.propertyName}, Keys: {curve.keys.Length}");
            foreach (var key in curve.keys)
            {
                Debug.LogWarning($"Time: {key.time}, Value: {key.value}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}