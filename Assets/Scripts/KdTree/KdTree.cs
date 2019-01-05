//////////////////////////////////////////////////////////////////////////////////////
// The MIT License(MIT)
// Copyright(c) 2018 lycoder

// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

//////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Linq;

namespace Kd_Tree
{
    public class KdTree
    {
        private KdTreeNode _treeNode;

        public KdTreeNode buildKdTree(List<TreeData> objList)
        {
            _treeNode = new KdTreeNode();

            _treeNode = buildKdTreeRecursive(objList);

            return _treeNode;
        }

        private KdTreeNode buildKdTreeRecursive(List<TreeData> objList)
        {
            if (objList.Count == 0)
            {
                return null;
            }

            KdTreeNode node = new KdTreeNode();
            if (objList.Count == 1)
            {
                node.m_Data = objList[0];
                node.optimalSplit = -1;
                return node;
            }

            // 计算最优分割项(最大方差法)
            int optimalSplit = GetOptimalSplitAxis(objList);
            //根据分割项，选出中位节点，分割出左右子树
            var sortList = objList.OrderBy(x => x.m_Value[optimalSplit]).ToList();
            int half = sortList.Count() / 2;
            List<TreeData> rightList = new List<TreeData>();
            List<TreeData> LeftList = new List<TreeData>();
            rightList.AddRange(sortList.Take(half));
            LeftList.AddRange(sortList.Skip(half));
            node.m_Data = LeftList[0];
            LeftList.RemoveAt(0);
            node.optimalSplit = optimalSplit;
            node.m_LeftNode = buildKdTreeRecursive(LeftList);
            node.m_RightNode = buildKdTreeRecursive(rightList);
            return node;
        }
        //计算最优分割项
        private int GetOptimalSplitAxis(List<TreeData> objList)
        {
            //(最大方差法)
            int optimalSplit = 0;
            int valueCount = objList[0].m_Value.Count;

            List<double> varList = new List<double>();
            for (int i = 0; i < valueCount; i++)
            {
                double ave = objList.Average(x => x.m_Value[i]);
                double sum = objList.Sum(x => (x.m_Value[i] - ave) * (x.m_Value[i] - ave));
                double var = sum / objList.Count;
                varList.Add(var);
            }
            for (int i = 1; i < varList.Count; i++)
            {
                if (varList[i] > varList[i - 1])
                {
                    optimalSplit = i;
                }
            }
            return optimalSplit;
        }
    }

    public class KdTreeNode
    {
        public TreeData m_Data;
        public int optimalSplit;
        public KdTreeNode m_LeftNode;
        public KdTreeNode m_RightNode;
    }

    public class TreeData
    {
        public List<int> m_Value;
    }
}
