using System.Collections.Generic;
using UnityEngine;
using Kd_Tree;
using UnityEngine.Profiling;

public class TestKdTree : MonoBehaviour {
    public int seed = 0;
    public int k = 3;
    public Transform dataParent;
    public Transform treeParent;
    List<TreeData> data = new List<TreeData>();
    void Start () {
        Random.InitState(seed);
        for (int i = 0; i < 10000; i++)
        {
            data.Add(new TreeData());
            data[i].m_Value = new List<int>();
            for (int j = 0; j < k; j++)
            {
                data[i].m_Value.Add(Random.Range(0, 100));
            }
        }
        KdTree kdtree = new KdTree();
        Profiler.BeginSample("buildKdTree");
        var tree = kdtree.buildKdTree(data);
        Profiler.EndSample();
        DebugTree(tree, treeParent);

        TreeData target = new TreeData();
        target.m_Value = new List<int>();
        for (int i = 0; i < k; i++)
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

        Debug.Log("target:" + target.ToString(-1));
        Debug.Log("norV3:" + data[minIndex].ToString(-1) + "dis:" + minDis);
        Debug.Log("nnV3:" + nearestNode.m_Data.ToString(-1) + " dis:" + nearestNode.Distance(target));
    }

    #region DEBUG
    void DebugTree(KdTreeNode root, Transform parent)
    {
        var obj = GenObj(root, parent);
        if (root.m_LeftNode != null)
        {
            DebugTree(root.m_LeftNode, obj.transform);
        }
        if (root.m_RightNode != null)
        {
            DebugTree(root.m_RightNode, obj.transform);
        }
    }
    GameObject GenObj(KdTreeNode root, Transform parent = null)
    {
        var obj = new GameObject(root.m_Data.ToString(root.optimalSplit));
        if (parent != null)
        {
            obj.transform.parent = parent;
        }
        return obj;
    }
    #endregion
}
