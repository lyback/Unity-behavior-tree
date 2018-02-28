using BTreeFrame;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BTree.Editor
{
    public enum NodeConnectionType
    {
        Incoming,
        Outgoing,
        Fixed
    }
    public class BTreeNodeConnection<T, P>
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {
        public BTreeNodeDesigner<T, P> m_DestinationNodeDesigner;
        public BTreeNodeDesigner<T, P> m_OriginatingNodeDesigner;
        public float m_HorizontalHeight;

        public NodeConnectionType m_NodeConnectionType;
        private bool m_Selected;

        public void loadConnection(BTreeNodeDesigner<T, P> nodeDesigner, NodeConnectionType nodeConnectionType)
        {
            m_OriginatingNodeDesigner = nodeDesigner;
            m_NodeConnectionType = nodeConnectionType;
            m_Selected = false;
        }
        //绘制连线
        public void drawConnection(Vector2 offset, float graphZoom, bool disabled)
        {
            drawConnection(m_OriginatingNodeDesigner.getConnectionPosition(offset, NodeConnectionType.Outgoing), m_DestinationNodeDesigner.getConnectionPosition(offset, NodeConnectionType.Incoming), graphZoom, disabled);
        }
        //绘制连线
        public void drawConnection(Vector2 source, Vector2 destination, float graphZoom, bool disabled)
        {
            Color color = disabled ? new Color(0.7f, 0.7f, 0.7f) : Color.white;
            Handles.color = color;
            Vector3[] array = new Vector3[]
            {
                source,
                new Vector2(source.x, this.m_HorizontalHeight),
                new Vector2(destination.x, this.m_HorizontalHeight),
                destination
            };
            Handles.DrawAAPolyLine(BTreeEditorUtility.TaskConnectionTexture, 1f / graphZoom, array);
        }
    }
}
