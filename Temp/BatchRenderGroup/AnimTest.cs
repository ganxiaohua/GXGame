using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using Random = System.Random;

public class AnimTest : MonoBehaviour
{
    public int keyt;
    void Start()
    {
        transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.SetColor("_BaseColor", (UnityEngine.Random.ColorHSV()));
        transform.GetComponent<Animator>().Play(keyt==1?"Hip Hop Dancing":"mixamo_com");
        transform.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
}