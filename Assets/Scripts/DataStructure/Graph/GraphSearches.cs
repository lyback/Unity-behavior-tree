using System.Collections.Generic;
public class Graph_SearchDFS<NODE, EDGE> where NODE : GraphNode where EDGE : GraphEdge, new()
{
    //to aid legibility
    enum VisitState
    {
        visited, unvisited, no_parent_assigned
    };
    //a reference to the graph to be searched
    BaseGraph<NODE, EDGE> m_Graph;

    //this records the indexes of all the nodes that are visited as the
    //search progresses
    List<VisitState> m_Visited;
    //this holds the route taken to the target. Given a node index, the value
    //at that index is the node's parent. ie if the path to the target is
    //3-8-27, then m_Route[8] will hold 3 and m_Route[27] will hold 8.
    List<int> m_Route;
    //As the search progresses, this will hold all the edges the algorithm has
    //examined. THIS IS NOT NECESSARY FOR THE SEARCH, IT IS HERE PURELY
    //TO PROVIDE THE USER WITH SOME VISUAL FEEDBACK
    List<EDGE> m_SpanningTree = new List<EDGE>();
    //the source and target node indices
    int m_iSource, m_iTarget;

    //true if a path to the target has been found
    bool m_bFound;

    public Graph_SearchDFS(BaseGraph<NODE, EDGE> graph, int source, int target)
    {
        m_Graph = graph;
        m_iSource = source;
        m_iTarget = target;
        m_bFound = false;
        m_Visited = new List<VisitState>(m_Graph.NodeCount);
        m_Route = new List<int>(m_Graph.NodeCount);
        for (int i = 0; i < m_Graph.NodeCount; i++)
        {
            m_Visited.Add(VisitState.unvisited);
            m_Route.Add(-1);
        }
        m_bFound = Search();
    }
    //this method performs the DFS search
    bool Search()
    {
        //create a std stack of edges
        Stack<EDGE> stack = new Stack<EDGE>();
        //create a dummy edge and put on the stack
        EDGE Dummy = new EDGE();
        Dummy.From = m_iSource;
        Dummy.To = m_iSource;
        Dummy.Cost = 0;
        stack.Push(Dummy);
        //while there are edges in the stack keep searching
        while (stack.Count > 0)
        {
            //get and remove the edge from the stack
            EDGE Next = stack.Pop();
            //make a note of the parent of the node this edge points to
            m_Route[Next.To] = Next.From;
            //put it on the tree. (making sure the dummy edge is not placed on the tree)
            if (Next != Dummy)
            {
                m_SpanningTree.Add(Next);
            }
            //and mark it visited
            m_Visited[Next.To] = VisitState.visited;
            //if the target has been found the method can return success
            if (Next.To == m_iTarget)
            {
                return true;
            }
            //push the edges leading from the node this edge points to onto
            //the stack (provided the edge does not point to a previously 
            //visited node)
            foreach (EDGE edge in m_Graph.Enumerator_Edge(Next.To))
            {
                if (m_Visited[edge.To] == VisitState.unvisited)
                {
                    stack.Push(edge);
                }
            }
        }
        //no path to target
        return false;
    }

    public bool IsFound()
    {
        return m_bFound;
    }

    public List<int> GetRoute()
    {
        return m_Route;
    }

    public List<EDGE> GetSpanningTree()
    {
        return m_SpanningTree;
    }
}