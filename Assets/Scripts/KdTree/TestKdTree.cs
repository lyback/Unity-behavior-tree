using System.Collections.Generic;
using UnityEngine;
using Kd_Tree;

public class TestKdTree : MonoBehaviour {

	// Use this for initialization
	void Start () {

        List<TreeData> data = new List<TreeData>();
        for (int i = 0; i < 100; i++)
        {
            data.Add(new TreeData());
            data[i].m_Value = new List<int>();
            for (int j = 0; j < 3; j++)
            {
                data[i].m_Value.Add(Random.Range(0, 1000));
            }
        }
        KdTree kdtree = new KdTree();
        var tree = kdtree.buildKdTree(data);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
