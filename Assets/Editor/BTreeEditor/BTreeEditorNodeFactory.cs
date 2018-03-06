using System;
using System.Collections.Generic;
using BTreeFrame;
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
            for (int i = 0; i < _nodeDesigners.Length; i++)
            {
                if (_nodeDesigners[i] == null)
                {
                    _nodeDesigners[i] = CreateBTreeNodeDesigner(_config.m_EditorNodes, _editorNodes, ref _nodeDesigners, i);
                }
            }
            for (int i = 0; i < _nodeDesigners.Length; i++)
            {
                BTreeEditorNodeConfig _configNode = _config.m_EditorNodes[i];
                if (_configNode.m_ParentIndex > 0)
                {
                    _nodeDesigners[i].m_ParentNode = _nodeDesigners[_configNode.m_ParentIndex];
                }
                _nodeDesigners[i].init();
            }
            return _nodeDesigners;
        }
        public static BTreeNodeDesigner<T, P> CreateBTreeNodeDesigner(BTreeEditorNodeConfig[] _configNodes, BTreeEditorNode<T, P>[] _editorNodes, ref BTreeNodeDesigner<T, P>[] _nodeDesigners, int _index)
        {
            BTreeEditorNode<T, P> _editorNode = _editorNodes[_index];
            BTreeEditorNodeConfig _configNode = _configNodes[_index];
            //if (_configNode.m_ParentIndex >= 0 && _nodeDesigners[_configNode.m_ParentIndex] == null)
            //{
            //    _nodeDesigners[_configNode.m_ParentIndex] = CreateBTreeNodeDesigner(_configNodes, _editorNodes, ref _nodeDesigners, _configNode.m_ParentIndex);
            //}
            for (int i = 0; i < _editorNode.m_Node.m_ChildCount; i++)
            {
                int _childIndex = _editorNode.m_Node.m_ChildNodeList[i].m_Index;
                if (_nodeDesigners[_childIndex] == null)
                {
                    _nodeDesigners[_childIndex] = CreateBTreeNodeDesigner(_configNodes, _editorNodes, ref _nodeDesigners, _childIndex);
                }
            }
            BTreeNodeDesigner<T, P> _node = new BTreeNodeDesigner<T, P>();
            _node.m_EditorNode = _editorNode;
            _node.m_NodeName = _editorNode.m_Node.m_Name;
            _node.m_ChildNodeList = new List<BTreeNodeDesigner<T, P>>();
            _node.m_ChildNodeConnectionList = new List<BTreeNodeConnection<T, P>>();
            for (int i = 0; i < _editorNode.m_Node.m_ChildCount; i++)
            {
                int _childIndex = _editorNode.m_Node.m_ChildNodeList[i].m_Index;
                _node.m_ChildNodeList.Add(_nodeDesigners[_childIndex]);
                BTreeNodeConnection<T, P> _connection = new BTreeNodeConnection<T, P>(_nodeDesigners[_childIndex], _nodeDesigners[_index], NodeConnectionType.Outgoing);
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
                _editorNodes[i].m_Pos = _config.m_EditorNodes[i].m_Pos;
                _editorNodes[i].m_Disable = _config.m_EditorNodes[i].m_Disable;
            }
            //for (int i = 0; i < _editorNodes.Length; i++)
            //{
            //    if (_btreeNodes[i].m_ChildCount != 0)
            //    {
            //        _editorNodes[i].m_ChildNodeList = new BTreeEditorNode<T, P>[_btreeNodes[i].m_ChildCount];
            //        for (int j = 0; i < _btreeNodes[i].m_ChildCount; j++)
            //        {
            //            int _index = _btreeNodes[i].m_ChildNodeList[j].m_Index;
            //            _editorNodes[i].m_ChildNodeList[j] = _editorNodes[_index];
            //        }
            //    }
            //}
            return _editorNodes;
        }
        #endregion
    }
}
