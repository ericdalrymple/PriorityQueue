using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// An implementation of a priority queue deliberately made from scratch.
/// Implemented in the form of a Binomial Heap.
/// </summary>
/// <typeparam name="T">A class.</typeparam>
class PriorityQueue<T>
{
    public enum SortMode
    {
          ASCENDING = 0
        , DESCENDING
    }

    /// <summary>
    /// Tuple node that can be compared to other nodes of its kind according to priority.
    /// </summary>
    /// <typeparam name="S"></typeparam>
    private class PriorityQueueTuple<S>
    : IComparable<PriorityQueueTuple<S>>
    {
        public T value { get { return m_Value; } }
        public int priority { get { return m_Priority; } }
        public PriorityQueueTuple<T> counterpart { get { return m_Counterpart; } set { m_Counterpart = value; } }

        private T m_Value;
        private int m_Priority = -1;
        private SortMode m_SortMode;
        private PriorityQueueTuple<T> m_Counterpart;

        public PriorityQueueTuple( SortMode mode, T value, int priority )
        {
            m_Value = value;
            m_Priority = priority;
            m_SortMode = mode;
        }

        public int CompareTo( PriorityQueueTuple<S> other )
        {
            if( SortMode.DESCENDING == m_SortMode )
            {
                return other.m_Priority - m_Priority;
            }

            return m_Priority - other.m_Priority;
        }
    }
    
    public T min
    {
        get
        {
            PriorityQueueTuple<T> result = m_MinHeap.Peek();
            if( null != result )
            {
                return result.value;
            }

            return default( T );
        }
    }

    private BinomialHeap<PriorityQueueTuple<T>> m_MinHeap = null;
    private BinomialHeap<PriorityQueueTuple<T>> m_MaxHeap = null;

    public PriorityQueue()
    {

    }

    public void Enqueue( T value, int priority )
    {
        //-- Create our tuple
        PriorityQueueTuple<T> newMinEntry = new PriorityQueueTuple<T>( SortMode.ASCENDING, value, priority );
        PriorityQueueTuple<T> newMaxEntry = new PriorityQueueTuple<T>( SortMode.DESCENDING, value, priority );

        //-- Link them so we can access them from each other
        newMinEntry.counterpart = newMaxEntry;
        newMaxEntry.counterpart = newMinEntry;

        //-- Add to the min heap
        BinomialHeap<PriorityQueueTuple<T>> newMinHeap = new BinomialHeap<PriorityQueueTuple<T>>( newMinEntry );
        if( null != m_MinHeap )
        {
            m_MinHeap = BinomialHeap<PriorityQueueTuple<T>>.Merge( m_MinHeap, newMinHeap );
        }
        else
        {
            m_MinHeap = newMinHeap;
        }

        //-- Add to the max heap
        BinomialHeap<PriorityQueueTuple<T>> newMaxHeap = new BinomialHeap<PriorityQueueTuple<T>>( newMaxEntry );
        if( null != m_MaxHeap )
        {
            m_MaxHeap = BinomialHeap<PriorityQueueTuple<T>>.Merge( m_MaxHeap, newMaxHeap );
        }
        else
        {
            m_MaxHeap = newMaxHeap;
        }
    }

    public void DequeueMin()
    {

    }

    public void DequeueMax()
    {

    }
}
