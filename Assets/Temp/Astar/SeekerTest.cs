using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Path = Pathfinding.Path;

public class SeekerTest : MonoBehaviour
{
    // Start is called before the first frame update
    public Seeker seeker;
    public Transform Transform;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        seeker.StartPath(transform.position, Transform.position, OnPath);
    }

    private void OnPath(Path a)
    {
    }
}