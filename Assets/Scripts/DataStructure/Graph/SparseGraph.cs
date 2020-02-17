using System.Collections;
using System.Collections.Generic;
public class SparseGraph<NODE, EDGE> : IEnumerable<NODE> where NODE : GraphNode where EDGE : GraphEdge, new()
{
    private List<NODE> m_Nodes;
    private List<List<EDGE>> m_Edges;
    private bool m_bDiGraph;
    private int m_iNextNodeIndex;

    public SparseGraph()
    {

    }

    //--------------------------- isNodePresent --------------------------------
    //
    //  returns true if a node with the given index is present in the graph
    //--------------------------------------------------------------------------
    public bool isNodePresent(int nd)
    {
        if ((m_Nodes[nd].m_iIndex == -1) || (nd >= m_Nodes.Count))
        {
            return false;
        }
        else return true;
    }
    //--------------------------- isEdgePresent --------------------------------
    //
    //  returns true if an edge with the given from/to is present in the graph
    //--------------------------------------------------------------------------
    public bool isEdgePresent(int from, int to)
    {
        if (isNodePresent(from) && isNodePresent(to))
        {
            for (int i = 0; i < m_Edges[from].Count; i++)
            {
                if (m_Edges[from][i].To == to)
                {
                    return true;
                }
            }
            return false;
        }
        else return false;
    }
    //------------------------------ GetNode -------------------------------------
    //
    //  clone and non clone methods for obtaining a reference to a specific node
    //----------------------------------------------------------------------------
    public NODE GetNode(int idx)
    {
        if (idx < 0 || idx >= m_Nodes.Count)
        {
            return null;
        }
        return m_Nodes[idx];
    }
    public NODE GetNode_Clone(int idx)
    {
        if (idx < 0 || idx >= m_Nodes.Count)
        {
            return null;
        }
        return m_Nodes[idx].Clone() as NODE;
    }
    //------------------------------ GetEdge -------------------------------------
    //
    //  clone and non clone methods for obtaining a reference to a specific edge
    //----------------------------------------------------------------------------
    EDGE GetEdge(int from, int to)
    {
        if (from < 0 || from >= m_Nodes.Count || to < 0 || to >= m_Nodes.Count)
        {
            return null;
        }
        var _edges = m_Edges[from];
        for (int i = 0; i < _edges.Count; i++)
        {
            if (_edges[i].To == to)
            {
                return _edges[i];
            }
        }
        return null;
    }
    EDGE GetEdge_Clone(int from, int to)
    {
        if (from < 0 || from >= m_Nodes.Count || to < 0 || to >= m_Nodes.Count)
        {
            return null;
        }
        var _edges = m_Edges[from];
        for (int i = 0; i < _edges.Count; i++)
        {
            if (_edges[i].To == to)
            {
                return _edges[i].Clone() as EDGE;
            }
        }
        return null;
    }

    //-------------------------- AddEdge ------------------------------------------
    //
    //  Use this to add an edge to the graph. The method will ensure that the
    //  edge passed as a parameter is valid before adding it to the graph. If the
    //  graph is a digraph then a similar edge connecting the nodes in the opposite
    //  direction will be automatically added.
    //-----------------------------------------------------------------------------
    void AddEdge(EDGE edge)
    {
        int _nodeCount = m_Nodes.Count;
        if (edge.From < 0 || edge.From > _nodeCount || edge.To < 0 || edge.To > _nodeCount)
        {
            return;
        }

        //add the edge, first making sure it is unique
        if (UniqueEdge(edge.From, edge.To))
        {
            m_Edges[edge.From].Add(edge);
        }

        //if the graph is undirected we must add another connection in the opposite
        //direction
        if (!m_bDiGraph)
        {
            //check to make sure the edge is unique before adding
            if (UniqueEdge(edge.To, edge.From))
            {
                EDGE NewEdge = edge;
                NewEdge.To = edge.From;
                NewEdge.From = edge.To;
                m_Edges[edge.To].Add(NewEdge);
            }
        }
    }
    //-------------------------------- UniqueEdge ----------------------------
    //
    //  returns true if the edge is not present in the graph. Used when adding
    //  edges to prevent duplication
    //------------------------------------------------------------------------
    bool UniqueEdge(int from, int to)
    {
        var _edges = m_Edges[from];
        if (_edges != null)
        {
            for (int i = 0; i < _edges.Count; i++)
            {
                if (_edges[i].To == to)
                {
                    return false;
                }
            }
        }
        return true;
    }
    //----------------------- CullInvalidEdges ------------------------------------
    //
    //  iterates through all the edges in the graph and removes any that point
    //  to an invalidated node
    //-----------------------------------------------------------------------------
    void CullInvalidEdges()
    {
        for (int i = 0; i < m_Edges.Count; i++)
        {
            var _edges = m_Edges[i];
            for (int j = _edges.Count - 1; j < 0; j--)
            {
                EDGE edge = _edges[j];
                if (edge.From == -1 && edge.To == -1)
                {
                    _edges.RemoveAt(j);
                }
            }
        }
    }


    public virtual IEnumerator<NODE> GetEnumerator()
    {
        if (m_Nodes == null)
            yield break;

        yield return m_Nodes[0];
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
