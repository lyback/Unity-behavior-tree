using System.Collections;
using System.Collections.Generic;

public class GraphEdge
{
    protected int m_iFrom;
    protected int m_iTo;

    public GraphEdge(int from, int to)
    {
        m_iFrom = from;
        m_iTo = to;
    }
}
