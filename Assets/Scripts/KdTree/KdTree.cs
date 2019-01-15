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

using System;
using System.Collections;
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
            //_treeNode = buildKdTreeRecursive(objList);
            _treeNode = buildKdTreeRecursive2(objList, 0, objList.Count); // 耗时更短，内存消耗更小
            return _treeNode;
        }
        //建树方法1
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
        //建树方法2
        private KdTreeNode buildKdTreeRecursive2(List<TreeData> objList, int startIndex, int length)
        {
            if (length == 0)
            {
                return null;
            }
            KdTreeNode node = new KdTreeNode();
            if (length == 1)
            {
                node.m_Data = objList[startIndex];
                node.optimalSplit = -1;
                return node;
            }
            // 计算最优分割项(最大方差法)
            int optimalSplit = GetOptimalSplitAxis(objList.GetRange(startIndex, length));
            //根据分割项，选出中位节点，分割出左右子树
            objList.Sort(startIndex, length, new KdTreeComparer(optimalSplit));
            int middleIndex = startIndex + length / 2;
            node.m_Data = objList[middleIndex];
            node.optimalSplit = optimalSplit;
            int leftStartIndex = startIndex;
            node.m_LeftNode = buildKdTreeRecursive2(objList, leftStartIndex, middleIndex - leftStartIndex);
            int rightStartIndex = middleIndex + 1;
            node.m_RightNode = buildKdTreeRecursive2(objList, rightStartIndex, startIndex + length - rightStartIndex);
            return node;
        }
        //计算最优分割项
        private int GetOptimalSplitAxis(List<TreeData> objList)
        {
            //(最大方差法)
            int optimalSplit = 0;
            int valueCount = objList[0].m_Value.Count;

            double maxVar = 0;
            for (int i = 0; i < valueCount; i++)
            {
                double ave = objList.Average(x => x.m_Value[i]);
                double sum = objList.Sum(x => (x.m_Value[i] - ave) * (x.m_Value[i] - ave));
                double var = sum / objList.Count;
                if (var > maxVar)
                {
                    maxVar = var;
                    optimalSplit = i;
                }
            }
            return optimalSplit;
        }
        //最近邻近点
        public KdTreeNode FindNearestNode(KdTreeNode root, TreeData target)
        {
            Stack<KdTreeNode> backStack = new Stack<KdTreeNode>();
            KdTreeNode nearestNode = DFSSearch(root, target, ref backStack);
            int minDis = nearestNode.Distance(target);
            while(backStack.Count != 0)
            {
                KdTreeNode backNode = backStack.Pop();
                int backDis = backNode.Distance(target);
                if (backDis < minDis)
                {
                    nearestNode = backNode;
                    minDis = backDis;
                }
                if (backNode.optimalSplit != -1)
                {
                    if (backNode.DisOfSplit(target) < minDis)
                    {
                        if (backNode.LeftOf(target) && backNode.m_RightNode != null)
                        {
                            DFSSearch(backNode.m_RightNode, target, ref backStack);
                        }
                        else if (backNode.RightOf(target) && backNode.m_LeftNode != null)
                        {
                            DFSSearch(backNode.m_LeftNode, target, ref backStack);
                        }
                    }
                }
            }
            return nearestNode;
        }
        //深度查找
        public KdTreeNode DFSSearch(KdTreeNode root, TreeData target, ref Stack<KdTreeNode> backStack)
        {
            if (backStack != null)
            {
                backStack.Push(root);
            }
            if (root.optimalSplit == -1)
            {
                return root;
            }
            if (root.LeftOf(target))
            {
                if (root.m_LeftNode == null)
                {
                    return root;
                }
                else
                {
                    return DFSSearch(root.m_LeftNode, target, ref backStack);
                }
            }
            else if(root.RightOf(target))
            {
                if (root.m_RightNode == null)
                {
                    return root;
                }
                else
                {
                    return DFSSearch(root.m_RightNode, target, ref backStack);
                }
            }
            else
            {
                if (root.Distance(target) == 0)
                {
                    return root;
                }
                if (root.m_LeftNode != null)
                {
                    return DFSSearch(root.m_LeftNode, target, ref backStack);
                }
                else if (root.m_RightNode != null)
                {
                    return DFSSearch(root.m_RightNode, target, ref backStack);
                }
                else
                {
                    return root;
                }
            }
        }
    }

    public class KdTreeNode
    {
        public TreeData m_Data;
        public int optimalSplit;
        public KdTreeNode m_LeftNode;
        public KdTreeNode m_RightNode;
        public KdTreeNode m_ParentNode;
        public bool m_IsSelfLeft;

        public bool LeftOf(TreeData data)
        {
            return m_Data.m_Value[optimalSplit] > data.m_Value[optimalSplit];
        }
        public bool RightOf(TreeData data)
        {
            return m_Data.m_Value[optimalSplit] < data.m_Value[optimalSplit];
        }
        public int Distance(TreeData data)
        {
            int dis = 0;
            for (int i = 0; i < m_Data.m_Value.Count; i++)
            {
                dis += Math.Abs(m_Data.m_Value[i] * m_Data.m_Value[i] - data.m_Value[i] * data.m_Value[i]);
            }
            return dis;
        }
        public int DisOfSplit(TreeData data)
        {
            return Math.Abs(m_Data.m_Value[optimalSplit] * m_Data.m_Value[optimalSplit] - data.m_Value[optimalSplit] * data.m_Value[optimalSplit]);
        }
    }

    public class TreeData
    {
        public List<int> m_Value;
        public int Distance(TreeData data)
        {
            int dis = 0;
            for (int i = 0; i < m_Value.Count; i++)
            {
                dis += Math.Abs(m_Value[i] * m_Value[i] - data.m_Value[i] * data.m_Value[i]);
            }
            return dis;
        }
    }

    public class KdTreeComparer : IComparer<TreeData>
    {
        private int optimalSplit;
        public KdTreeComparer(int _optimalSplit)
        {
            optimalSplit = _optimalSplit;
        }
        public int Compare(TreeData x, TreeData y)
        {
            return x.m_Value[optimalSplit].CompareTo(y.m_Value[optimalSplit]);
        }
    }
}
