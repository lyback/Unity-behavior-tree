using System.Collections;
using System.Collections.Generic;
using System;

public class GraphNode : System.ICloneable
{
    public GraphNode()
    {

    }
    public int Index
    {
        get;
        set;
    }

    public GraphNode(int index)
    {
        Index = index;
    }

    public virtual Object Clone()
    {
        return this.MemberwiseClone();
    }
}
