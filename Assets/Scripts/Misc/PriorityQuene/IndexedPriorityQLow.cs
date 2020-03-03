//----------------------- IndexedPriorityQLow ---------------------------
//
//  Priority queue based on an index into a set of keys. The queue is
//  maintained as a 2-way heap.
//
//  The priority in this implementation is the lowest valued key
//------------------------------------------------------------------------
using System.Collections.Generic;
using System;
class IndexedPriorityQLow<T> where T : IComparable<T>
{
    List<T> m_vecKeys;
    List<int> m_Heap;
    List<int> m_invHeap;
    int m_iSize;
    int m_iMaxSize;

    public IndexedPriorityQLow(int MaxSize)
    {
        m_iSize = 0;
        m_iMaxSize = MaxSize;
        m_vecKeys = new List<T>(MaxSize);
        m_Heap = new List<int>(MaxSize);
        m_invHeap = new List<int>(MaxSize);
        for (int i = 0; i < MaxSize; i++)
        {
            m_vecKeys.Add(default(T));
            m_Heap.Add(-1);
            m_invHeap.Add(-1);
        }
    }

    public bool empty()
    {
        return m_iSize == 0;
    }

    //to insert an item into the queue it gets added to the end of the heap
    //and then the heap is reordered from the bottom up.
    public void insert(int idx, T key)
    {
        if (m_iSize + 1 > m_iMaxSize)
        {
            return;
        }
        ++m_iSize;
        m_vecKeys[idx] = key;
        m_Heap[m_iSize] = idx;
        m_invHeap[idx] = m_iSize;
        ReorderUpwards(m_iSize);
    }

    //to get the min item the first element is exchanged with the lowest
    //in the heap and then the heap is reordered from the top down. 
    public int Pop()
    {
        Swap(1, m_iSize);

        ReorderDownwards(1, m_iSize - 1);

        return m_Heap[m_iSize--];
    }

    //if the value of one of the client key's changes then call this with 
    //the key's index to adjust the queue accordingly
    public void ChangePriority(int idx, T key)
    {
        m_vecKeys[idx] = key;
        ReorderUpwards(m_invHeap[idx]);
    }

    void Swap(int a, int b)
    {
        int temp = m_Heap[a];
        m_Heap[a] = m_Heap[b];
        m_Heap[b] = temp;
        //change the handles too
        m_invHeap[m_Heap[a]] = a;
        m_invHeap[m_Heap[b]] = b;
    }
    void ReorderUpwards(int nd)
    {
        //move up the heap swapping the elements until the heap is ordered
        while ((nd > 1) && (m_vecKeys[m_Heap[nd / 2]].CompareTo(m_vecKeys[m_Heap[nd]]) > 0))
        {
            Swap(nd / 2, nd);
            nd /= 2;
        }
    }

    void ReorderDownwards(int nd, int HeapSize)
    {
        //move down the heap from node nd swapping the elements until
        //the heap is reordered
        while (2 * nd <= HeapSize)
        {
            int child = 2 * nd;

            //set child to smaller of nd's two children
            if ((child < HeapSize) && (m_vecKeys[m_Heap[child]].CompareTo(m_vecKeys[m_Heap[child + 1]]) > 0))
            {
                ++child;
            }

            //if this nd is larger than its child, swap
            if (m_vecKeys[m_Heap[nd]].CompareTo(m_vecKeys[m_Heap[child]]) > 0)
            {
                Swap(child, nd);

                //move the current node down the tree
                nd = child;
            }
            else
            {
                break;
            }
        }
    }
}