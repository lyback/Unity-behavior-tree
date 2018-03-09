using System;
using System.Collections.Generic;
using BTreeFrame;
using UnityEngine;

namespace BTree.Editor
{
    class BTreeEditorNodeFactory<T, P>
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {
        #region 从配置生成行为树编辑器相关方法
        public static BTreeNodeDesigner<T, P>[] CreateBTreeNodeDesignerFromConfig(BTreeEditorTreeConfig _config)
        {
            BTreeNodeDesigner<T, P>[] _nodeDesigners = new BTreeNodeDesigner<T, P>[_config.m_EditorNodes.Length];
            BTreeEditorNode<T, P>[] _editorNodes = CreateBTreeEditorNode(_config);
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
                    BTreeNodeConnection<T, P> _connection = new BTreeNodeConnection<T, P>(_nodeDesigners[i], _nodeDesigners[_parentIndex], NodeConnectionType.Incoming);
                    _nodeDesigners[i].m_ParentNodeConnection = _connection;
                }
            }
            return _nodeDesigners;
        }
        public static BTreeNodeDesigner<T, P> CreateBTreeNodeDesigner(BTreeEditorNodeConfig[] _configNodes, BTreeEditorNode<T, P>[] _editorNodes, ref BTreeNodeDesigner<T, P>[] _nodeDesigners, int _index)
        {
            BTreeEditorNode<T, P> _editorNode = _editorNodes[_index];
            for (int i = 0; i < _editorNode.m_Node.m_ChildCount; i++)
            {
                int _childIndex = _editorNode.m_Node.m_ChildNodeList[i].m_Index;
                if (_nodeDesigners[_childIndex] == null)
                {
                    _nodeDesigners[_childIndex] = CreateBTreeNodeDesigner(_configNodes, _editorNodes, ref _nodeDesigners, _childIndex);
                }
            }
            BTreeNodeDesigner<T, P> _node = new BTreeNodeDesigner<T, P>(_editorNode);
            //_node.m_EditorNode = _editorNode;
            //_node.m_NodeName = _editorNode.m_Node.m_Name;
            //_node.m_ChildNodeList = new List<BTreeNodeDesigner<T, P>>();
            //_node.m_ChildNodeConnectionList = new List<BTreeNodeConnection<T, P>>();
            
            for (int i = 0; i < _editorNode.m_Node.m_ChildCount; i++)
            {
                int _childIndex = _editorNode.m_Node.m_ChildNodeList[i].m_Index;
                _node.m_ChildNodeList.Add(_nodeDesigners[_childIndex]);
                BTreeNodeConnection<T, P> _connection = new BTreeNodeConnection<T, P>(_nodeDesigners[_childIndex], _node, NodeConnectionType.Outgoing);
                _node.m_ChildNodeConnectionList.Add(_connection);
            }
            return _node;
        }
        public static BTreeEditorNode<T, P>[] CreateBTreeEditorNode(BTreeEditorTreeConfig _config)
        {
            BTreeNodeFactory<T, P>.Init();
            BTreeNode<T, P>[] _btreeNodes = BTreeNodeFactory<T, P>.CreateBTreeFromConfig(_config);
            BTreeEditorNode<T, P>[] _editorNodes = new BTreeEditorNode<T, P>[_btreeNodes.Length];
            for (int i = 0; i < _editorNodes.Length; i++)
            {
                _editorNodes[i] = new BTreeEditorNode<T, P>(_btreeNodes[i]);
                _editorNodes[i].m_Pos = new Vector2(_config.m_EditorNodes[i].m_PosX, _config.m_EditorNodes[i].m_PosY);
                _editorNodes[i].m_Disable = _config.m_EditorNodes[i].m_Disable;
            }
            return _editorNodes;
        }
        #endregion

        #region 从行为树编辑器类生成配置
        public static BTreeEditorConfig CreateBtreeEditorConfigFromGraphDesigner(BTreeGraphDesigner<T, P> _graphDesigner)
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
        public static BTreeEditorTreeConfig CreateEditorTreeConfigFromRootEditorNode(BTreeNodeDesigner<T, P> _rootEditorNode)
        {
            TreeConfig _treeConfig = BTreeNodeFactory<T, P>.CreateConfigFromBTreeRoot(_rootEditorNode.m_EditorNode.m_Node);
            BTreeEditorTreeConfig _treeEditorConfig = new BTreeEditorTreeConfig(_treeConfig);
            CreateEditorNodeConfigFromRootEditorNode(_rootEditorNode, ref _treeEditorConfig.m_EditorNodes);
            
            return _treeEditorConfig;
        }
        public static BTreeEditorNodeConfig CreateEditorNodeConfigFromRootEditorNode(BTreeNodeDesigner<T, P> _rootEditorNode, ref BTreeEditorNodeConfig[] _editorNodes)
        {
            int _index = _rootEditorNode.m_EditorNode.m_Node.m_Index;
            _editorNodes[_index].m_PosX = _rootEditorNode.m_EditorNode.m_Pos.x;
            _editorNodes[_index].m_PosY = _rootEditorNode.m_EditorNode.m_Pos.y;
            _editorNodes[_index].m_Disable = _rootEditorNode.m_EditorNode.m_Disable;
            if (_rootEditorNode.m_ChildNodeList != null)
            {
                for (int i = 0; i < _rootEditorNode.m_ChildNodeList.Count; i++)
                {
                    CreateEditorNodeConfigFromRootEditorNode(_rootEditorNode.m_ChildNodeList[i], ref _editorNodes);
                }
            }
            return _editorNodes[_index];
        }
        #endregion
        #region 从行为树编辑器类生成运行时配置
        public static TreeConfig CreateTreeConfigFromBTreeGraphDesigner(BTreeGraphDesigner<T, P> _graphDesigner)
        {
            BTreeNode<T, P> _root = _graphDesigner.m_RootNode.m_EditorNode.m_Node;
            return BTreeNodeFactory<T, P>.CreateConfigFromBTreeRoot(_root);
        }
        #endregion
    }
}
