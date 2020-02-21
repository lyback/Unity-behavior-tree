using System.Collections;
using System.Collections.Generic;

public class SparseGraph<NODE, EDGE> : BaseGraph<NODE, EDGE> where NODE : GraphNode where EDGE : GraphEdge, new()
{
    public SparseGraph()
    {

    }

    //--------------------------- isNodePresent --------------------------------
    //
    //  returns true if a node with the given index is present in the graph
    //--------------------------------------------------------------------------
    public bool isNodePresent(int nd)
    {
        if ((m_Nodes[nd].Index == -1) || (nd >= m_Nodes.Count))
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
    public EDGE GetEdge(int from, int to)
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
    public EDGE GetEdge_Clone(int from, int to)
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
    public void AddEdge(EDGE edge)
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
                EDGE NewEdge = edge.Clone() as EDGE;
                NewEdge.To = edge.From;
                NewEdge.From = edge.To;
                m_Edges[edge.To].Add(NewEdge);
            }
        }
    }

    //----------------------------- RemoveEdge ---------------------------------
    public void RemoveEdge(int from, int to)
    {
        if (from >= m_Nodes.Count || to >= m_Nodes.Count)
        {
            return;
        }

        if (!m_bDiGraph)
        {
            for (int i = 0; i < m_Edges[to].Count; i++)
            {
                if (m_Edges[to][i].To == from)
                {
                    m_Edges[to].RemoveAt(i);
                }
            }
        }
        for (int j = 0; j < m_Edges[from].Count; j++)
        {
            if (m_Edges[from][j].To == to)
            {
                m_Edges[from].RemoveAt(j);
            }
        }
    }


    //-------------------------- AddNode -------------------------------------
    //
    //  Given a node this method first checks to see if the node has been added
    //  previously but is now innactive. If it is, it is reactivated.
    //
    //  If the node has not been added previously, it is checked to make sure its
    //  index matches the next node index before being added to the graph
    //------------------------------------------------------------------------
    public int AddNode(NODE node)
    {
        if (node.Index < (int)m_Nodes.Count)
        {
            //make sure the client is not trying to add a node with the same ID as
            //a currently active node
            if (m_Nodes[node.Index].Index == -1)
            {
                return -1;
            }
            m_Nodes[node.Index] = node;

            return m_iNextNodeIndex;
        }
        else
        {
            //make sure the new node has been indexed correctly
            if (node.Index == m_iNextNodeIndex)
            {
                return -1;
            }

            m_Nodes.Add(node);
            m_Edges.Add(new List<EDGE>());

            return m_iNextNodeIndex++;
        }
    }


    //------------------------------- RemoveNode -----------------------------
    //
    //  Removes a node from the graph and removes any links to neighbouring
    //  nodes
    //------------------------------------------------------------------------
    public void RemoveNode(int node)
    {
        if (node >= m_Nodes.Count)
        {
            return;
        }

        //set this node's index to invalid_node_index
        m_Nodes[node].Index = -1;

        //if the graph is not directed remove all edges leading to this node and then
        //clear the edges leading from the node
        if (!m_bDiGraph)
        {
            //visit each neighbour and erase any edges leading to this node
            for (int i = 0; i < m_Edges[node].Count; i++)
            {
                var _to = m_Edges[node][i].To;
                for (int j = 0; j < m_Edges[_to].Count; j++)
                {
                    if (m_Edges[_to][j].To == node)
                    {
                        m_Edges[_to].RemoveAt(j);
                        break;
                    }
                }
            }
            //finally, clear this node's edges
            m_Edges[node].Clear();
        }
        //if a digraph remove the edges the slow way
        else
        {
            CullInvalidEdges();
        }
    }

    //-------------------------- SetEdgeCost ---------------------------------
    //
    //  Sets the cost of a specific edge
    //------------------------------------------------------------------------
    public void SetEdgeCost(int from, int to, double NewCost)
    {
        //make sure the nodes given are valid
        if (from >= m_Nodes.Count || to >= m_Nodes.Count)
        {
            return;
        }
        //visit each neighbour and erase any edges leading to this node
        for (int i = 0; i < m_Edges[from].Count; i++)
        {
            if (m_Edges[from][i].To == to)
            {
                m_Edges[from][i].Cost = NewCost;
                break;
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
        if (from < 0 || from >= m_Edges.Count)
        {
            return false;
        }
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

}
