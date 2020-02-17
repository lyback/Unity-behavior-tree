using System.Collections;
using System.Collections.Generic;
using System;

public class GraphEdge : System.ICloneable
{
    public int From{
        get;
        set;
    }
    public int To{
        get;
        set;
    }

    public GraphEdge(int from, int to)
    {
        From = from;
        To = to;
    }


    public virtual double Distance(){
        return 0;
    }

    
    public virtual Object Clone()
    {
        return this.MemberwiseClone();
    }
}
