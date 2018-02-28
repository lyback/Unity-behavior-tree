using System.Collections.Generic;
using BTreeFrame;
using UnityEngine;

namespace BTree.Editor
{
    public class BTreeNodeDesigner<T, P>
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {
        public BTreeEditorNode<T, P> m_Node { get; private set; }
        private BTreeNodeDesigner<T, P> m_ParentNode;
        public List<BTreeNodeDesigner<T, P>> m_ChildNodeList;
        public List<BTreeNodeConnection<T, P>> m_ChildNodeConnectionList;
        private string m_NodeName = "";
        private bool m_Selected;
        private bool m_IsDirty;
        private bool m_IsEntryDisplay;
        public bool m_IsParent { get; private set; }
        private bool m_IsShowHoverBar;
        private Texture m_Icon;
        public void select()
        {
            if (!m_IsEntryDisplay)
            {
                m_Selected = true;
            }
        }
        public void deselect()
        {
            m_Selected = false;
        }
        public void markDirty()
        {
            m_IsDirty = true;
        }
        public Rect IncomingConnectionRect(Vector2 offset)
        {
            Rect rect = this.rectangle(offset, false);
            return new Rect(rect.x + (rect.width - (float)BTreeEditorUtility.ConnectionWidth) / 2f, rect.y - (float)BTreeEditorUtility.TopConnectionHeight, (float)BTreeEditorUtility.ConnectionWidth, (float)BTreeEditorUtility.TopConnectionHeight);
        }
        public Rect OutgoingConnectionRect(Vector2 offset)
        {
            Rect rect = this.rectangle(offset, false);
            return new Rect(rect.x + (rect.width - (float)BTreeEditorUtility.ConnectionWidth) / 2f, rect.yMax, (float)BTreeEditorUtility.ConnectionWidth, (float)BTreeEditorUtility.BottomConnectionHeight);
        }
        
        public void loadNode(BTreeEditorNode<T, P> node, ref int id)
        {
            if (node == null)
            {
                return;
            }
            m_Node = node;
            loadTaskIcon();
            init();
            
        }
        //绘制节点
        public bool drawNode(Vector2 offset, bool drawSelected, bool disabled)
        {
            if (drawSelected != this.m_Selected)
            {
                return false;
            }

            Rect rect = this.rectangle(offset, false);

            GUI.color = Color.white;
            if (this.m_IsShowHoverBar)
            {
                GUI.DrawTexture(new Rect(rect.x - 1f, rect.y - 17f, 14f, 14f), this.m_Node.m_Disable ? BTreeEditorUtility.EnableTaskTexture : BTreeEditorUtility.DisableTaskTexture, ScaleMode.ScaleToFit);
                if (this.m_IsParent)
                {
                    GUI.DrawTexture(new Rect(rect.x + 15f, rect.y - 17f, 14f, 14f), this.m_Node.m_IsCollapsed ? BTreeEditorUtility.ExpandTaskTexture : BTreeEditorUtility.CollapseTaskTexture, ScaleMode.ScaleToFit);
                }
            }
            return true;
        }
        //绘制连线
        public void drawNodeConnection(Vector2 offset, float graphZoom, bool disabled)
        {
            if (m_IsDirty)
            {
                determineConnectionHorizontalHeight(this.rectangle(offset, false), offset);
                m_IsDirty = false;
            }
            if (m_IsParent)
            {
                for (int i = 0; i < this.m_ChildNodeConnectionList.Count; i++)
                {
                    m_ChildNodeConnectionList[i].drawConnection(offset, graphZoom, disabled);
                }
            }
        }
        //绘制节点说明
        public void drawNodeComment(Vector2 offset)
        {
            
        }
        //获取连线位置
        public Vector2 getConnectionPosition(Vector2 offset, NodeConnectionType connectionType)
        {
            Vector2 result;
            if (connectionType == NodeConnectionType.Incoming)
            {
                Rect rect = this.IncomingConnectionRect(offset);
                result = new Vector2(rect.center.x, rect.y + (float)(BTreeEditorUtility.TopConnectionHeight / 2));
            }
            else
            {
                Rect rect2 = this.OutgoingConnectionRect(offset);
                result = new Vector2(rect2.center.x, rect2.yMax - (float)(BTreeEditorUtility.BottomConnectionHeight / 2));
            }
            return result;
        }
        
        private void loadTaskIcon()
        {
            string _iconName = "{SkinColor}ActionIcon.png";
            m_Icon = BTreeEditorUtility.LoadIcon(_iconName);
        }
        private void init()
        {
            m_NodeName = m_Node.m_BTreeNode.m_Name;
            m_IsParent = m_Node.m_BTreeNode.m_ChildCount != 0;
            if (m_IsParent)
            {
                m_ChildNodeConnectionList = new List<BTreeNodeConnection<T,P>>();
                m_ChildNodeList = new List<BTreeNodeDesigner<T, P>>();
            }
        }
        private Rect rectangle(Vector2 offset, bool includeConnections)
        {
            Rect result = this.rectangle(offset);
            if (includeConnections)
            {
                if (!this.m_IsEntryDisplay)
                {
                    result.yMin = (result.yMin - (float)BTreeEditorUtility.TopConnectionHeight);
                }
                if (this.m_IsParent)
                {
                    result.yMax = (result.yMax + (float)BTreeEditorUtility.BottomConnectionHeight);
                }
            }
            return result;
        }
        private Rect rectangle(Vector2 offset)
        {
            if (this.m_Node == null)
            {
                return default(Rect);
            }
            float num = BTreeEditorUtility.TaskTitleGUIStyle.CalcSize(new GUIContent(this.ToString())).x + (float)BTreeEditorUtility.TextPadding;
            if (!m_IsParent)
            {
                float num2;
                float num3;
                BTreeEditorUtility.TaskCommentGUIStyle.CalcMinMaxWidth(new GUIContent("Comment(Test)"), out num2, out num3);
                num3 += (float)BTreeEditorUtility.TextPadding;
                num = ((num > num3) ? num : num3);
            }
            num = Mathf.Min((float)BTreeEditorUtility.MaxWidth, Mathf.Max((float)BTreeEditorUtility.MinWidth, num));
            return new Rect(this.m_Node.m_Pos.x + offset.x - num / 2f, this.m_Node.m_Pos.y + offset.y, num, (float)(BTreeEditorUtility.IconAreaHeight + BTreeEditorUtility.TitleHeight));
        }
        //确定连线横向高度
        private void determineConnectionHorizontalHeight(Rect nodeRect, Vector2 offset)
        {
            if (this.m_IsParent)
            {
                float num = 3.40282347E+38f;
                float num2 = num;
                for (int i = 0; i < this.m_ChildNodeConnectionList.Count; i++)
                {
                    Rect rect = this.m_ChildNodeConnectionList[i].m_DestinationNodeDesigner.rectangle(offset, false);
                    if (rect.y < num)
                    {
                        num = rect.y;
                        num2 = rect.y;
                    }
                }
                num = num * 0.75f + nodeRect.yMax * 0.25f;
                if (num < nodeRect.yMax + 15f)
                {
                    num = nodeRect.yMax + 15f;
                }
                else if (num > num2 - 15f)
                {
                    num = num2 - 15f;
                }
                for (int j = 0; j < this.m_ChildNodeConnectionList.Count; j++)
                {
                    this.m_ChildNodeConnectionList[j].m_HorizontalHeight = num;
                }
            }
        }
    }
}
