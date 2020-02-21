using System.Collections;
using System.Collections.Generic;
using System;

public class GraphEdge : System.ICloneable
{
    public GraphEdge()
    {

    }
    public int From
    {
        get;
        set;
    }
    public int To
    {
        get;
        set;
    }
    public double Cost
    {
        get;
        set;
    }

    public GraphEdge(int from, int to, int cost)
    {
        From = from;
        To = to;
        Cost = cost;
    }


    public virtual double Distance()
    {
        return 0;
    }


    public virtual Object Clone()
    {
        return this.MemberwiseClone();
    }
}
