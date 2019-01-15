using System.Collections.Generic;
using UnityEngine;
using Kd_Tree;
using UnityEngine.Profiling;

public class TestKdTree : MonoBehaviour {
    public int seed = 0;
    public Transform dataParent;
    public Transform treeParent;
    List<TreeData> data = new List<TreeData>();
	// Use this for initialization
    void Awake()
    {
        DebugData();
    }
    void Start () {
        Random.InitState(seed);
        for (int i = 0; i < 10000; i++)
        {
            data.Add(new TreeData());
            data[i].m_Value = new List<int>();
            for (int j = 0; j < 3; j++)
            {
                data[i].m_Value.Add(Random.Range(0, 100));
            }
        }
        KdTree kdtree = new KdTree();
        Profiler.BeginSample("buildKdTree");
        var tree = kdtree.buildKdTree(data);
        Profiler.EndSample();
        //DebugTree(tree, treeParent);

        TreeData target = new TreeData();
        target.m_Value = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            target.m_Value.Add(Random.Range(0, 100));
        }

        Profiler.BeginSample("FindNearestNode");
        var nearestNode = kdtree.FindNearestNode(tree, target);
        Profiler.EndSample();

        Profiler.BeginSample("NorFind");
        int minDis = int.MaxValue;
        int minIndex = 0;
        for (int i = 0; i < data.Count; i++)
        {
            int dis = data[i].Distance(target);
            if (dis < minDis)
            {
                minDis = dis;
                minIndex = i;
            }
        }
        Profiler.EndSample();

        Vector3 targetV3 = new Vector3(target.m_Value[0], target.m_Value[1], target.m_Value[2]);
        Vector3 norV3 = new Vector3(data[minIndex].m_Value[0], data[minIndex].m_Value[1], data[minIndex].m_Value[2]);
        Vector3 nnV3 = new Vector3(nearestNode.m_Data.m_Value[0], nearestNode.m_Data.m_Value[1], nearestNode.m_Data.m_Value[2]);
        GenObj(targetV3, PrimitiveType.Capsule);
        GenObj(nnV3, PrimitiveType.Sphere);
        GenObj(norV3, PrimitiveType.Cylinder);
        Debug.Log("target:"+targetV3);
        Debug.Log("norV3:"+ norV3 + " dis:" + Vector3.Distance(norV3, targetV3));
        Debug.Log("nnV3:" + nnV3 + " dis:" + Vector3.Distance(nnV3, targetV3));
    }
	void DebugTree(KdTreeNode root, Transform parent)
    {
        var pos = root.m_Data.m_Value;
        Vector3 v3 = new Vector3(pos[0], pos[1], pos[2]);
        var obj = GenObj(v3, PrimitiveType.Cube, parent);
        if (root.m_LeftNode != null)
        {
            DebugTree(root.m_LeftNode, obj.transform);
        }
        if (root.m_RightNode != null)
        {
            DebugTree(root.m_RightNode, obj.transform);
        }
    }
    void DebugData()
    {
        for (int i = 0; i < data.Count; i++)
        {
            Vector3 v3 = new Vector3(data[i].m_Value[0], data[i].m_Value[1], data[i].m_Value[2]);
            GenObj(v3, PrimitiveType.Cube, dataParent);
        }
    }
    GameObject GenObj(Vector3 v3, PrimitiveType type, Transform parent = null)
    {
        var obj = GameObject.CreatePrimitive(type);
        if (parent != null)
        {
            obj.transform.parent = parent;
        }
        obj.transform.position = v3;
        return obj;
    }
}
