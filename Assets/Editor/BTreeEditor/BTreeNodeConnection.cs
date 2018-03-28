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
    public class BTreeNodeConnection
    {
        public BTreeNodeDesigner m_DestinationNodeDesigner;
        public BTreeNodeDesigner m_OriginatingNodeDesigner;
        public float m_HorizontalHeight;

        public NodeConnectionType m_NodeConnectionType;
        private bool m_Selected;
        
        public BTreeNodeConnection(BTreeNodeDesigner _dest, BTreeNodeDesigner _orig, NodeConnectionType _type)
        {
            m_DestinationNodeDesigner = _dest;
            m_OriginatingNodeDesigner = _orig;
            m_NodeConnectionType = _type;
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
                new Vector2(source.x, m_HorizontalHeight),
                new Vector2(destination.x, m_HorizontalHeight),
                destination
            };
            Handles.DrawAAPolyLine(BTreeEditorUtility.TaskConnectionTexture, 1f / graphZoom, array);
        }
        
    }
}
