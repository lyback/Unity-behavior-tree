using System.Collections.Generic;

public class BaseGraph<NODE, EDGE> where NODE : GraphNode where EDGE : GraphEdge, new()
{
    protected List<NODE> m_Nodes = new List<NODE>();
    public int NodeCount
    {
        get
        {
            if (m_Nodes != null)
            {
                return m_Nodes.Count;
            }
            return 0;
        }
    }
    protected List<List<EDGE>> m_Edges = new List<List<EDGE>>();
    public int EdgeCount
    {
        get
        {
            if (m_Edges != null)
            {
                return m_Edges.Count;
            }
            return 0;
        }
    }
    protected bool m_bDiGraph;
    protected int m_iNextNodeIndex = -1;

    //----------------------- Enumerator ------------------------------------
    public IEnumerable<NODE> Enumerator_Node()
    {
        for (int i = 0; i < m_Nodes.Count; i++)
        {
            yield return m_Nodes[i];
        }
    }
    public IEnumerable<EDGE> Enumerator_Edge(int index = 0){
        for (int i = 0; i < m_Edges[index].Count; i++)
        {
            yield return m_Edges[index][i];
        }
    }
    public IEnumerable<EDGE> Enumerator_Edge()
    {
        foreach (var edges in m_Edges)
        {
            foreach (var edge in edges)
            {
                yield return edge;
            }
        }
    }
}