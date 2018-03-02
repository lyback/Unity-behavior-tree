using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using BTreeFrame;
namespace BTree.Editor
{
    public class BTreeGraphDesigner<T, P>
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {
        public BTreeNodeDesigner<T, P> m_EntryNode { get; private set; }
        public BTreeNodeDesigner<T, P> m_RootNode { get; private set; }
        public List<BTreeNodeDesigner<T, P>> m_DetachedNodes = new List<BTreeNodeDesigner<T, P>>();
        public List<BTreeNodeDesigner<T, P>> m_SelectedNodes = new List<BTreeNodeDesigner<T, P>>();
        public BTreeNodeDesigner<T, P> m_HoverNode { get; private set; }
        public BTreeNodeConnection<T, P> m_ActiveNodeConnection { get; set; }
        public List<BTreeNodeConnection<T, P>> m_SelectedNodeConnections = new List<BTreeNodeConnection<T, P>>();

        public bool drawNodes(Vector2 mousePosition, Vector2 offset, float graphZoom)
        {
            if (this.m_EntryNode == null)
            {
                return false;
            }
            m_EntryNode.drawNodeConnection(offset, graphZoom, false);
            //从根节点开始递归绘制
            if (m_RootNode != null)
            {
                drawNodeConnectionChildren(m_RootNode, offset, graphZoom, m_RootNode.m_Node.m_Disable);
            }
            //绘制未连接的节点
            for (int i = 0; i < m_DetachedNodes.Count; i++)
            {
                drawNodeConnectionChildren(m_DetachedNodes[i], offset, graphZoom, m_DetachedNodes[i].m_Node.m_Disable);
            }
            //绘制选中的连线
            for (int i = 0; i < m_SelectedNodeConnections.Count; i++)
            {
                m_SelectedNodeConnections[i].drawConnection(offset, graphZoom, m_SelectedNodeConnections[i].m_OriginatingNodeDesigner.m_Node.m_Disable);
            }
            //
            if (mousePosition != new Vector2(-1f, -1f) && m_ActiveNodeConnection != null)
            {
                m_ActiveNodeConnection.m_HorizontalHeight = (m_ActiveNodeConnection.m_OriginatingNodeDesigner.getConnectionPosition(offset, m_ActiveNodeConnection.m_NodeConnectionType).y + mousePosition.y) / 2;
                var _offset = m_ActiveNodeConnection.m_OriginatingNodeDesigner.getConnectionPosition(offset, m_ActiveNodeConnection.m_NodeConnectionType);
                var _disable = m_ActiveNodeConnection.m_OriginatingNodeDesigner.m_Node.m_Disable && m_ActiveNodeConnection.m_NodeConnectionType == NodeConnectionType.Outgoing;
                m_ActiveNodeConnection.drawConnection(_offset, mousePosition, graphZoom, _disable);
            }
            m_EntryNode.drawNode(offset, false, false);
            bool result = false;
            //绘制跟节点
            if (m_RootNode != null && drawNodeChildren(m_RootNode, offset, m_RootNode.m_Node.m_Disable))
            {
                result = true;
            }
            //绘制未连接的节点
            for (int i = 0; i < m_DetachedNodes.Count; i++)
            {
                if (drawNodeChildren(m_DetachedNodes[i], offset, m_DetachedNodes[i].m_Node.m_Disable))
                {
                    result = true;
                }
            }
            //绘制选中的节点
            for (int i = 0; i < m_SelectedNodes.Count; i++)
            {
                if (drawNodeChildren(m_SelectedNodes[i], offset, m_SelectedNodes[i].m_Node.m_Disable))
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
        private void drawNodeConnectionChildren(BTreeNodeDesigner<T, P> nodeDesigner, Vector2 offset, float graphZoom, bool disabledNode)
        {
            if (nodeDesigner == null)
            {
                return;
            }
            if (!nodeDesigner.m_Node.m_IsCollapsed)
            {
                nodeDesigner.drawNodeConnection(offset, graphZoom, nodeDesigner.m_Node.m_Disable || disabledNode);
                for (int i = 0; i < nodeDesigner.m_ChildNodeList.Count; i++)
                {
                    var _child = nodeDesigner.m_ChildNodeList[i];
                    drawNodeConnectionChildren(_child, offset, graphZoom, _child.m_Node.m_Disable || disabledNode);
                }
            }
        }
        //递归绘制节点说明
        private void drawNodeCommentChildren(BTreeNodeDesigner<T, P> nodeDesigner, Vector2 offset)
        {
            if (nodeDesigner == null)
            {
                return;
            }
            nodeDesigner.drawNodeComment(offset);
            for (int i = 0; i < nodeDesigner.m_ChildNodeList.Count; i++)
            {
                var _child = nodeDesigner.m_ChildNodeList[i];
                drawNodeCommentChildren(_child, offset);
            }
        }
        //递归绘制节点
        private bool drawNodeChildren(BTreeNodeDesigner<T, P> nodeDesigner, Vector2 offset, bool disabledNode)
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
            for (int i = 0; i < nodeDesigner.m_ChildNodeList.Count; i++)
            {
                var _child = nodeDesigner.m_ChildNodeList[i];
                if (drawNodeChildren(_child, offset, _child.m_Node.m_Disable))
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
