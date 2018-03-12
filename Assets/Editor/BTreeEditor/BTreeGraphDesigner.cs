using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using BTreeFrame;
using System;
using Battle.Logic.AI.BTree;

namespace BTree.Editor
{
    public class BTreeGraphDesigner
    {
        //public BTreeNodeDesigner m_EntryNode { get; private set; }
        public BTreeNodeDesigner m_RootNode { get; private set; }
        public List<BTreeNodeDesigner> m_DetachedNodes = new List<BTreeNodeDesigner>();
        public List<BTreeNodeDesigner> m_SelectedNodes = new List<BTreeNodeDesigner>();
        public BTreeNodeDesigner m_HoverNode { get; private set; }
        public BTreeNodeConnection m_ActiveNodeConnection { get; set; }
        public List<BTreeNodeConnection> m_SelectedNodeConnections = new List<BTreeNodeConnection>();

        #region 绘制相关
        public bool drawNodes(Vector2 mousePosition, Vector2 offset, float graphZoom)
        {
            //if (m_EntryNode == null)
            //{
            //    return false;
            //}
            //m_EntryNode.drawNodeConnection(offset, graphZoom, false);
            //从根节点开始递归绘制
            
            if (m_RootNode != null)
            {
                drawNodeConnectionChildren(m_RootNode, offset, graphZoom, m_RootNode.m_EditorNode.m_Disable);
            }
            //绘制未连接的节点
            for (int i = 0; i < m_DetachedNodes.Count; i++)
            {
                drawNodeConnectionChildren(m_DetachedNodes[i], offset, graphZoom, m_DetachedNodes[i].m_EditorNode.m_Disable);
            }
            //绘制选中的连线
            for (int i = 0; i < m_SelectedNodeConnections.Count; i++)
            {
                m_SelectedNodeConnections[i].drawConnection(offset, graphZoom, m_SelectedNodeConnections[i].m_OriginatingNodeDesigner.m_EditorNode.m_Disable);
            }
            //
            if (mousePosition != new Vector2(-1f, -1f) && m_ActiveNodeConnection != null)
            {
                m_ActiveNodeConnection.m_HorizontalHeight = (m_ActiveNodeConnection.m_OriginatingNodeDesigner.getConnectionPosition(offset, m_ActiveNodeConnection.m_NodeConnectionType).y + mousePosition.y) / 2;
                var _offset = m_ActiveNodeConnection.m_OriginatingNodeDesigner.getConnectionPosition(offset, m_ActiveNodeConnection.m_NodeConnectionType);
                var _disable = m_ActiveNodeConnection.m_OriginatingNodeDesigner.m_EditorNode.m_Disable && m_ActiveNodeConnection.m_NodeConnectionType == NodeConnectionType.Outgoing;
                m_ActiveNodeConnection.drawConnection(_offset, mousePosition, graphZoom, _disable);
            }
            
            //m_EntryNode.drawNode(offset, false, false);
            bool result = false;
            //绘制跟节点
            if (m_RootNode != null && drawNodeChildren(m_RootNode, offset, m_RootNode.m_EditorNode.m_Disable))
            {
                result = true;
            }
            //绘制未连接的节点
            for (int i = 0; i < m_DetachedNodes.Count; i++)
            {
                if (drawNodeChildren(m_DetachedNodes[i], offset, m_DetachedNodes[i].m_EditorNode.m_Disable))
                {
                    result = true;
                }
            }
            //绘制选中的节点
            for (int i = 0; i < m_SelectedNodes.Count; i++)
            {
                if (drawNodeChildren(m_SelectedNodes[i], offset, m_SelectedNodes[i].m_EditorNode.m_Disable))
                {
                    result = true;
                }
            }
            //绘制根节点的说明
            if (m_RootNode != null)
            {
                drawNodeCommentChildren(m_RootNode, offset);
            }
            //绘制分离节点说明
            for (int i = 0; i < m_DetachedNodes.Count; i++)
            {
                drawNodeCommentChildren(m_DetachedNodes[i], offset);
            }
            return result;
        }
        //递归绘制连线
        private void drawNodeConnectionChildren(BTreeNodeDesigner nodeDesigner, Vector2 offset, float graphZoom, bool disabledNode)
        {
            if (nodeDesigner == null)
            {
                return;
            }
            if (!nodeDesigner.m_EditorNode.m_IsCollapsed)
            {
                nodeDesigner.drawNodeConnection(offset, graphZoom, nodeDesigner.m_EditorNode.m_Disable || disabledNode);
                if (nodeDesigner.m_ChildNodeList != null)
                {
                    for (int i = 0; i < nodeDesigner.m_ChildNodeList.Count; i++)
                    {
                        var _child = nodeDesigner.m_ChildNodeList[i];
                        drawNodeConnectionChildren(_child, offset, graphZoom, _child.m_EditorNode.m_Disable || disabledNode);
                    }
                }
            }
        }
        //递归绘制节点说明
        private void drawNodeCommentChildren(BTreeNodeDesigner nodeDesigner, Vector2 offset)
        {
            if (nodeDesigner == null)
            {
                return;
            }
            nodeDesigner.drawNodeComment(offset);
            if (nodeDesigner.m_ChildNodeList != null)
            {
                for (int i = 0; i < nodeDesigner.m_ChildNodeList.Count; i++)
                {
                    var _child = nodeDesigner.m_ChildNodeList[i];
                    drawNodeCommentChildren(_child, offset);
                }
            }
        }
        //递归绘制节点
        private bool drawNodeChildren(BTreeNodeDesigner nodeDesigner, Vector2 offset, bool disabledNode)
        {
            if (nodeDesigner == null)
            {
                return false;
            }
            bool result = false;
            if (nodeDesigner.drawNode(offset, false, disabledNode))
            {
                result = true;
            }
            if (nodeDesigner.m_ChildNodeList != null)
            {
                for (int i = 0; i < nodeDesigner.m_ChildNodeList.Count; i++)
                {
                    var _child = nodeDesigner.m_ChildNodeList[i];
                    if (drawNodeChildren(_child, offset, _child.m_EditorNode.m_Disable))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        #endregion
        //加载
        public void load(BTreeEditorConfig _config)
        {
            m_RootNode = BTreeEditorNodeFactory.CreateBTreeNodeDesignerFromConfig(_config.m_RootNode)[0];
            if (_config.m_DetachedNode != null)
            {
                m_DetachedNodes = new List<BTreeNodeDesigner>();
                for (int i = 0; i < _config.m_DetachedNode.Count; i++)
                {
                    BTreeNodeDesigner _detachedNode = BTreeEditorNodeFactory.CreateBTreeNodeDesignerFromConfig(_config.m_DetachedNode[i])[0];
                    m_DetachedNodes.Add(_detachedNode);
                }
            }
        }
        //获取鼠标位置上的节点
        public BTreeNodeDesigner nodeAt(Vector2 point, Vector2 offset)
        {
            for (int i = 0; i < m_SelectedNodes.Count; i++)
            {
                if (m_SelectedNodes[i].contains(point, offset, false))
                {
                    return m_SelectedNodes[i];
                }
            }
            BTreeNodeDesigner result;
            if (m_RootNode != null)
            {
                result = nodeChildrenAt(m_RootNode, point, offset);
                if (result != null)
                {
                    return result;
                }
            }
            for (int j = 0; j < m_DetachedNodes.Count; j++)
            {
                if (m_DetachedNodes[j] != null)
                {
                    result = nodeChildrenAt(m_DetachedNodes[j], point, offset);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }
        public BTreeNodeDesigner nodeChildrenAt(BTreeNodeDesigner nodeDesigner, Vector2 point, Vector2 offset)
        {
            if (nodeDesigner.contains(point, offset, true))
            {
                return nodeDesigner;
            }
            if (nodeDesigner.m_IsParent)
            {
                if (nodeDesigner.m_ChildNodeList != null)
                {
                    for (int i = 0; i < nodeDesigner.m_ChildNodeList.Count; i++)
                    {
                        BTreeNodeDesigner result;
                        if (nodeDesigner.m_ChildNodeList[i] != null)
                        {
                            result = nodeChildrenAt(nodeDesigner.m_ChildNodeList[i], point, offset);
                            if (result != null)
                            {
                                return result;
                            }
                        }
                    }
                }
            }
            return null;
        }
        //选中
        public void select(BTreeNodeDesigner nodeDesigner)
        {
            m_SelectedNodes.Add(nodeDesigner);
            nodeDesigner.select();
        }
        //取消选中
        public void deselect(BTreeNodeDesigner nodeDesigner)
        {
            m_SelectedNodes.Remove(nodeDesigner);
            nodeDesigner.deselect();
        }
        //取消所有选中
        public void clearNodeSelection()
        {
            if (m_SelectedNodes != null)
            {
                for (int i = 0; i < m_SelectedNodes.Count; i++)
                {
                    m_SelectedNodes[i].deselect();
                }
                m_SelectedNodes.Clear();
            }
        }
        //禁用节点
        public void disableNodeSelection()
        {
            if (m_SelectedNodes != null)
            {
                for (int i = 0; i < m_SelectedNodes.Count; i++)
                {
                    SetNodeActive(m_SelectedNodes[i], false);
                }
            }
        }
        //启用节点
        public void enableNodeSelection()
        {
            if (m_SelectedNodes != null)
            {
                for (int i = 0; i < m_SelectedNodes.Count; i++)
                {
                    SetNodeActive(m_SelectedNodes[i], true);
                }
            }
        }
        private void SetNodeActive(BTreeNodeDesigner nodeDesigner, bool isEnable)
        {
            if (isEnable)
            {
                nodeDesigner.enable();
            }
            else
            {
                nodeDesigner.disable();
            }
            if (nodeDesigner.m_ChildNodeList != null)
            {
                for (int i = 0; i < nodeDesigner.m_ChildNodeList.Count; i++)
                {
                    SetNodeActive(nodeDesigner.m_ChildNodeList[i], isEnable);
                }
            }
        }
        //删除选定节点
        public void delectSelectNode()
        {
            for (int i = 0; i < m_SelectedNodes.Count; i++)
            {
                delectNode(m_SelectedNodes[i]);
            }
            clearNodeSelection();
        }
        //拖动选择的节点
        public bool dragSelectedNodes(Vector2 delta, bool dragChildren, bool hasDragged)
        {
            if (m_SelectedNodes.Count == 0)
            {
                return false;
            }
            bool flag = m_SelectedNodes.Count == 1;
            for (int i = 0; i < m_SelectedNodes.Count; i++)
            {
                dragTask(m_SelectedNodes[i], delta, dragChildren, hasDragged);
            }
            //if (flag && dragChildren && m_RootNode != null)
            //{
            //    dragTask(m_RootNode, delta, dragChildren, hasDragged);
            //}
            return true;
        }
        private void dragTask(BTreeNodeDesigner nodeDesigner, Vector2 delta, bool dragChildren, bool hasDragged)
        {
            nodeDesigner.movePosition(delta);
            if (nodeDesigner.m_IsParent && dragChildren)
            {
                for (int i = 0; i < nodeDesigner.m_ChildNodeList.Count; i++)
                {
                    dragTask(nodeDesigner.m_ChildNodeList[i], delta, dragChildren, hasDragged);
                }
            }
        }
        //添加节点
        public BTreeNodeDesigner addNode(Type type, Vector2 position)
        {
            BTreeNode _node = (BTreeNode)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
            BTreeEditorNode _editorNode = new BTreeEditorNode(_node);
            _editorNode.m_Pos = position;
            BTreeNodeDesigner _nodeDesigner = new BTreeNodeDesigner(_editorNode);
            if (m_DetachedNodes == null)
            {
                m_DetachedNodes = new List<BTreeNodeDesigner>();
            }
            m_DetachedNodes.Add(_nodeDesigner);
            return _nodeDesigner;
        }
        //删除节点
        public void delectNode(BTreeNodeDesigner nodeDesigner)
        {
            if (nodeDesigner.m_IsParent)
            {
                for (int i = 0; i < nodeDesigner.m_ChildNodeConnectionList.Count; i++)
                {
                    BTreeNodeDesigner _destinationNodeDesigner = nodeDesigner.m_ChildNodeConnectionList[i].m_DestinationNodeDesigner;
                    m_DetachedNodes.Add(_destinationNodeDesigner);
                    _destinationNodeDesigner.m_ParentNode = null;
                }
            }
            if (nodeDesigner.m_ParentNode != null)
            {
                nodeDesigner.m_ParentNode.delectChildNode(nodeDesigner);
            }
            m_DetachedNodes.Remove(nodeDesigner);
            if (m_RootNode.Equals(nodeDesigner))
            {
                m_RootNode = null;
            }
        }
    }
}
