using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Obj
{
    public int id;
    public Rect rect;
}

public class QuadTreeNode
{
    public QuadTreeNode parent;
    public int depth;
    public Rect rect;
    public List<Obj> GoList = new();
    public List<QuadTreeNode> nodes;
}

public class QuadTree
{
    //可以划分的最多节点
    private const int MaxDepth = 3;
    private const int MaxSplit = 2;
    public QuadTreeNode quadTreeNode;

    public QuadTree()
    {
        quadTreeNode = new QuadTreeNode();
        quadTreeNode.rect = new Rect(0, 0, 10, 10);
        quadTreeNode.depth = 0;
    }

    public void AddGameObject(Obj obj, QuadTreeNode n = null)
    {
        QuadTreeNode node = n ?? quadTreeNode;
        if (node.rect.Overlaps(obj.rect))
        {
            if ((node.GoList.Count < MaxSplit && node.nodes == null) || node.depth == MaxDepth)
            {
                node.GoList.Add(obj);
            }
            else
            {
                Split(node);
                if (node.nodes == null)
                    return;
                foreach (var s in node.nodes)
                {
                    AddGameObject(obj, s);
                }
            }
        }
    }

    private void Split(QuadTreeNode n)
    {
        if (n.depth < MaxDepth && n.nodes == null)
        {
            float neww = n.rect.width / 2;
            float newh = n.rect.height / 2;
            n.nodes = new List<QuadTreeNode>();
            for (int i = 0; i < 4; i++)
            {
                var quadTreeNode = new QuadTreeNode();
                Rect rect = new Rect(n.rect.x + i % 2 * neww, n.rect.y + (int) (i / 2) * newh, neww, newh);
                quadTreeNode.rect = rect;
                quadTreeNode.depth = n.depth + 1;
                n.nodes.Add(quadTreeNode);
                quadTreeNode.parent = n;
            }

            foreach (var variable in n.GoList)
            {
                AddGameObject(variable, n);
            }

            n.GoList.Clear();
        }
    }
}

public class QuadtreeCollision : MonoBehaviour
{
    public QuadTree quad;

    void Start()
    {
        quad = new QuadTree();
        for (int i = 0; i < 20; i++)
        {
            quad.AddGameObject(new Obj() {id = i, rect = new Rect(Random.Range(0,9.0f), Random.Range(0,9.0f), 1, 1)});
        }

        // quad.AddGameObject(new Obj() {id = 4, rect = new Rect(0, 0, 2, 1)});
        // quad.AddGameObject(new Obj() {id = 5, rect = new Rect(0, 0, 2, 2)});
        // quad.AddGameObject(new Obj() {id = 6, rect = new Rect(0, 0, 2, 3)});
    }

    // Update is called once per frame
    void Update()
    {
        // CreateNode(quad.quadTreeNode);
        // Handles.DrawWireCube(Vector3.zero, new Vector3(5, 5, 1));
    }
    
}


[CustomEditor(typeof(QuadtreeCollision))]
public class WireBoxExample : Editor
{
    
    void OnSceneGUI()
    {
        if(target == null)
            return;
        QuadtreeCollision myObj = (QuadtreeCollision)target;
        if(myObj.quad==null)
            return;
        Node(myObj.quad.quadTreeNode);
    }
    
    private void Node(QuadTreeNode node)
    {
        Handles.DrawWireCube(node.rect.center, new Vector3(node.rect.width, node.rect.height, 1));
        Handles.color = Color.green;
        foreach (var variable in node.GoList)
        {
            Handles.DrawWireCube(variable.rect.center, new Vector3(variable.rect.width, variable.rect.height, 1));
        }
        Handles.color = Color.white;
        if (node.nodes == null)
            return;
        foreach (var item in node.nodes)
        {
            Handles.DrawWireCube(item.rect.center, new Vector3(item.rect.width, item.rect.height, 1));
            Node(item);
        }
    }
}