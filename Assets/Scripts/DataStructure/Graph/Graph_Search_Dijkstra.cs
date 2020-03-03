using System.Collections.Generic;
public class Graph_Search_Dijkstra<NODE, EDGE> where NODE : GraphNode where EDGE : GraphEdge, new()
{
    //to aid legibility
    enum VisitState
    {
        visited, unvisited, no_parent_assigned
    };
    //a reference to the graph to be searched
    BaseGraph<NODE, EDGE> m_Graph;

    //this vector contains the edges that comprise the shortest path tree -
    //a directed subtree of the graph that encapsulates the best paths from 
    //every node on the SPT to the source node.
    List<EDGE> m_ShortestPathTree;
    //this is indexed into by node index and holds the total cost of the best
    //path found so far to the given node. For example, m_CostToThisNode[5]
    //will hold the total cost of all the edges that comprise the best path
    //to node 5, found so far in the search (if node 5 is present and has 
    //been visited)
    List<double> m_CostToThisNode;
    //this is an indexed (by node) vector of 'parent' edges leading to nodes 
    //connected to the SPT but that have not been added to the SPT yet. This is
    //a little like the stack or queue used in BST and DST searches.
    List<EDGE> m_SearchFrontier;
    //the source and target node indices
    int m_iSource, m_iTarget;

    //true if a path to the target has been found
    bool m_bFound;

    public Graph_Search_Dijkstra(BaseGraph<NODE, EDGE> graph, int source, int target)
    {
        m_Graph = graph;
        m_iSource = source;
        m_iTarget = target;
        m_bFound = false;
        m_ShortestPathTree = new List<EDGE>();
        m_CostToThisNode = new List<double>();
        m_SearchFrontier = new List<EDGE>(graph.NodeCount);
        for (int i = 0; i < graph.NodeCount; i++)
        {
            m_SearchFrontier.Add(null);
        }
        m_bFound = Search();
    }
    //this method performs the DFS search
    bool Search()
    {
        //create an indexed priority queue that sorts smallest to largest
        //(front to back).Note that the maximum number of elements the iPQ
        //may contain is N. This is because no node can be represented on the 
        //queue more than once.
        IndexedPriorityQLow<double> pq = new IndexedPriorityQLow<double>(m_Graph.NodeCount);

        //put the source node on the queue
        pq.insert(m_iSource, 0f);

        //while the queue is not empty
        while (!pq.empty())
        {
            //get lowest cost node from the queue. Don't forget, the return value
            //is a *node index*, not the node itself. This node is the node not already
            //on the SPT that is the closest to the source node
            int NextClosestNode = pq.Pop();

            //move this edge from the frontier to the shortest path tree
            m_ShortestPathTree[NextClosestNode] = m_SearchFrontier[NextClosestNode];

            //if the target has been found exit
            if (NextClosestNode == m_iTarget) return true;

            //for each edge connected to the next closest node
            foreach (EDGE pE in m_Graph.Enumerator_Edge(NextClosestNode))
            {
                //the total cost to the node this edge points to is the cost to the
                //current node plus the cost of the edge connecting them.
                double NewCost = m_CostToThisNode[NextClosestNode] + pE.Cost;

                //if this edge has never been on the frontier make a note of the cost
                //to get to the node it points to, then add the edge to the frontier
                //and the destination node to the PQ.
                if (m_SearchFrontier[pE.To] == null)
                {
                    m_CostToThisNode[pE.To] = NewCost;

                    pq.insert(pE.To, NewCost);

                    m_SearchFrontier[pE.To] = pE;
                }
                //else test to see if the cost to reach the destination node via the
                //current node is cheaper than the cheapest cost found so far. If
                //this path is cheaper, we assign the new cost to the destination
                //node, update its entry in the PQ to reflect the change and add the
                //edge to the frontier
                else if ((NewCost < m_CostToThisNode[pE.To]) &&
                          (m_ShortestPathTree[pE.To] == null))
                {
                    m_CostToThisNode[pE.To] = NewCost;

                    //because the cost is less than it was previously, the PQ must be
                    //re-sorted to account for this.
                    pq.ChangePriority(pE.To, NewCost);

                    m_SearchFrontier[pE.To] = pE;
                }
            }
        }
        return false;
    }

    public bool IsFound()
    {
        return m_bFound;
    }

    public List<int> GetPathToTarget()
    {
        List<int> path = new List<int>();
        //just return an empty path if no target or no path found
        if (m_iTarget < 0 || !m_bFound) return path;

        int nd = m_iTarget;

        path.Add(nd);

        while ((nd != m_iSource) && (m_ShortestPathTree[nd] != null))
        {
            nd = m_ShortestPathTree[nd].From;

            path.Add(nd);
        }

        return path;
    }

}