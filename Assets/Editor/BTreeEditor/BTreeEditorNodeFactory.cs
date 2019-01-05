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
using BTreeFrame;
using UnityEngine;

namespace BTree.Editor
{
    class BTreeEditorNodeFactory
    {
        #region 从配置生成行为树编辑器相关方法
        public static BTreeNodeDesigner[] CreateBTreeNodeDesignerFromConfig(BTreeEditorTreeConfig _config)
        {
            BTreeNodeDesigner[] _nodeDesigners = new BTreeNodeDesigner[_config.m_EditorNodes.Length];
            BTreeEditorNode[] _editorNodes = CreateBTreeEditorNode(_config);
            //递归创建节点
            for (int i = 0; i < _nodeDesigners.Length; i++)
            {
                if (_nodeDesigners[i] == null)
                {
                    _nodeDesigners[i] = CreateBTreeNodeDesigner(_config.m_EditorNodes, _editorNodes, ref _nodeDesigners, i);
                }
            }
            //初始化父节点与连线
            for (int i = 0; i < _nodeDesigners.Length; i++)
            {
                var _editorNode = _editorNodes[i];
                if (_editorNode.m_Node.m_ParentNode != null)
                {
                    int _parentIndex = _editorNode.m_Node.m_ParentNode.m_Index;
                    _nodeDesigners[i].m_ParentNode = _nodeDesigners[_parentIndex];
                    BTreeNodeConnection _connection = new BTreeNodeConnection(_nodeDesigners[i], _nodeDesigners[_parentIndex], NodeConnectionType.Incoming);
                    _nodeDesigners[i].m_ParentNodeConnection = _connection;
                }
            }
            return _nodeDesigners;
        }
        public static BTreeNodeDesigner CreateBTreeNodeDesigner(BTreeEditorNodeConfig[] _configNodes, BTreeEditorNode[] _editorNodes, ref BTreeNodeDesigner[] _nodeDesigners, int _index)
        {
            BTreeEditorNode _editorNode = _editorNodes[_index];
            for (int i = 0; i < _editorNode.m_Node.m_ChildCount; i++)
            {
                int _childIndex = _editorNode.m_Node.m_ChildNodes[i].m_Index;
                if (_nodeDesigners[_childIndex] == null)
                {
                    _nodeDesigners[_childIndex] = CreateBTreeNodeDesigner(_configNodes, _editorNodes, ref _nodeDesigners, _childIndex);
                }
            }
            BTreeNodeDesigner _node = new BTreeNodeDesigner(_editorNode);
            //_node.m_EditorNode = _editorNode;
            //_node.m_NodeName = _editorNode.m_Node.m_Name;
            //_node.m_ChildNodeList = new List<BTreeNodeDesigner>();
            //_node.m_ChildNodeConnectionList = new List<BTreeNodeConnection>();
            
            for (int i = 0; i < _editorNode.m_Node.m_ChildCount; i++)
            {
                int _childIndex = _editorNode.m_Node.m_ChildNodes[i].m_Index;
                _node.m_ChildNodeList.Add(_nodeDesigners[_childIndex]);
                BTreeNodeConnection _connection = new BTreeNodeConnection(_nodeDesigners[_childIndex], _node, NodeConnectionType.Outgoing);
                _node.m_ChildNodeConnectionList.Add(_connection);
            }
            return _node;
        }
        public static BTreeEditorNode[] CreateBTreeEditorNode(BTreeEditorTreeConfig _config)
        {
            BTreeNodeFactory.Init();
            BTreeNode[] _btreeNodes = BTreeNodeFactory.CreateBTreeFromConfig(_config);
            BTreeEditorNode[] _editorNodes = new BTreeEditorNode[_btreeNodes.Length];
            for (int i = 0; i < _editorNodes.Length; i++)
            {
                _editorNodes[i] = new BTreeEditorNode(_btreeNodes[i]);
                _editorNodes[i].m_Pos = new Vector2(_config.m_EditorNodes[i].m_PosX, _config.m_EditorNodes[i].m_PosY);
                _editorNodes[i].m_Disable = _config.m_EditorNodes[i].m_Disable;
            }
            return _editorNodes;
        }
        #endregion

        #region 从行为树编辑器类生成配置
        public static BTreeEditorConfig CreateBtreeEditorConfigFromGraphDesigner(BTreeGraphDesigner _graphDesigner)
        {
            BTreeEditorConfig _config = new BTreeEditorConfig();
            
            _config.m_RootNode = CreateEditorTreeConfigFromRootEditorNode(_graphDesigner.m_RootNode);
            _config.m_RootNode.m_IsEnterNode = true;

            _config.m_DetachedNode = new List<BTreeEditorTreeConfig>();
            for (int i = 0; i < _graphDesigner.m_DetachedNodes.Count; i++)
            {
                _config.m_DetachedNode.Add(CreateEditorTreeConfigFromRootEditorNode(_graphDesigner.m_DetachedNodes[i]));
            }
            return _config;
        }
        public static BTreeEditorTreeConfig CreateEditorTreeConfigFromRootEditorNode(BTreeNodeDesigner _rootEditorNode)
        {
            TreeConfig _treeConfig = BTreeNodeFactory.CreateConfigFromBTreeRoot(_rootEditorNode.m_EditorNode.m_Node);
            BTreeEditorTreeConfig _treeEditorConfig = new BTreeEditorTreeConfig(_treeConfig);
            int index = 0;
            CreateEditorNodeConfigFromRootEditorNode(_rootEditorNode, ref _treeEditorConfig.m_EditorNodes, ref index);
            
            return _treeEditorConfig;
        }
        public static BTreeEditorNodeConfig CreateEditorNodeConfigFromRootEditorNode(BTreeNodeDesigner _rootEditorNode, ref BTreeEditorNodeConfig[] _editorNodes, ref int _index)
        {
            //int _index = _rootEditorNode.m_EditorNode.m_Node.m_Index;
            _editorNodes[_index].m_PosX = _rootEditorNode.m_EditorNode.m_Pos.x;
            _editorNodes[_index].m_PosY = _rootEditorNode.m_EditorNode.m_Pos.y;
            _editorNodes[_index].m_Disable = _rootEditorNode.m_EditorNode.m_Disable;
            if (_rootEditorNode.m_ChildNodeList != null)
            {
                for (int i = 0; i < _rootEditorNode.m_ChildNodeList.Count; i++)
                {
                    _index = _index + 1;
                    CreateEditorNodeConfigFromRootEditorNode(_rootEditorNode.m_ChildNodeList[i], ref _editorNodes, ref _index);
                }
            }
            return _editorNodes[_index];
        }
        #endregion
        #region 从行为树编辑器类生成运行时配置
        public static TreeConfig CreateTreeConfigFromBTreeGraphDesigner(BTreeGraphDesigner _graphDesigner)
        {
            BTreeNode _root = _graphDesigner.m_RootNode.m_EditorNode.m_Node;
            return BTreeNodeFactory.CreateConfigFromBTreeRoot(_root);
        }
        #endregion
    }
}
