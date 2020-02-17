using System.Collections;
using System.Collections.Generic;
using System;

public class GraphNode : System.ICloneable
{
    public int m_iIndex
    {
        get;
        protected set;
    }

    public GraphNode(int index)
    {
        m_iIndex = index;
    }

    public virtual Object Clone()
    {
        return this.MemberwiseClone();
    }
}
