using Pathfinding;
using UnityEngine;

public class AStarTest : MonoBehaviour
{
    // Start is called before the first frame update
    // public AIPath aiPath;
    // public Vector3 vec;
    public FollowerEntity Seeker;
    public Transform Transform;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Seeker.destination = Transform.position;
    }
}