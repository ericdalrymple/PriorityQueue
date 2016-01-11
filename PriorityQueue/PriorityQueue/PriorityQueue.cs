using System;
using System.Runtime.CompilerServices;

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
    public class PriorityQueueTuple
    : IComparable<PriorityQueueTuple>
    {
        public T Value { get { return m_Value; } }
        public int Priority { get { return m_Priority; } }

        private T m_Value;
        private int m_Priority = -1;
        private SortMode m_SortMode;

        /// <summary>
        /// Instantiate a new priority queue tuple.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="value"></param>
        /// <param name="priority"></param>
        public PriorityQueueTuple( SortMode mode, T value, int priority )
        {
            m_Value = value;
            m_Priority = priority;
            m_SortMode = mode;
        }

        /// <summary>
        /// Compares one tuple against another according to its sort mode.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo( PriorityQueueTuple other )
        {
            if( SortMode.DESCENDING == m_SortMode )
            {
                return other.m_Priority - m_Priority;
            }

            return m_Priority - other.m_Priority;
        }

        /// <summary>
        /// Returns a display string for this tuple.
        /// </summary>
        /// <returns>A display string.</returns>
        public override string ToString()
        {
            return "(" + m_Value.ToString() + ", " + m_Priority + ")";
        }
    }

    public T Max
    {
        get
        {
            PriorityQueueTuple result = m_MaxHeap.Peek();
            if( null != result )
            {
                return result.Value;
            }

            return default( T );
        }
    }

    public T Min
    {
        get
        {
            PriorityQueueTuple result = m_MinHeap.Peek();
            if( null != result )
            {
                return result.Value;
            }

            return default( T );
        }
    }

    private BinomialHeap<PriorityQueueTuple> m_MinHeap = null;
    private BinomialHeap<PriorityQueueTuple> m_MaxHeap = null;

    /// <summary>
    /// Adds a value with a specified priority to the priority queue.
    /// </summary>
    /// <remarks>
    /// Complexity: O(log n)
    /// </remarks>
    /// <param name="value">Value to be added.</param>
    /// <param name="priority">Priority of the specified value.</param>
    public void Enqueue( T value, int priority )
    {
        //-- Create our tuples
        PriorityQueueTuple newMinEntry = new PriorityQueueTuple( SortMode.ASCENDING, value, priority );
        PriorityQueueTuple newMaxEntry = new PriorityQueueTuple( SortMode.DESCENDING, value, priority );

        //-- Add to the min heap
        BinomialHeap<PriorityQueueTuple> newMinHeap = new BinomialHeap<PriorityQueueTuple>( newMinEntry );
        

        //-- Add to the max heap
        BinomialHeap<PriorityQueueTuple> newMaxHeap = new BinomialHeap<PriorityQueueTuple>( newMaxEntry );

        //-- Link the new nodes together so we can access them from each other
        newMinHeap.TreeList.First.Value.Root.m_Counterpart = newMaxHeap.TreeList.First.Value.Root;
        newMaxHeap.TreeList.First.Value.Root.m_Counterpart = newMinHeap.TreeList.First.Value.Root;
        
        //-- Integrate the new nodes into the existing heaps
        if( null != m_MinHeap )
        {
            m_MinHeap.Union( newMinHeap );
        }
        else
        {
            m_MinHeap = newMinHeap;
        }

        if( null != m_MaxHeap )
        {
            m_MaxHeap.Union( newMaxHeap );
        }
        else
        {
            m_MaxHeap = newMaxHeap;
        }
    }

    public void DequeueMin()
    {
        if( (null == m_MinHeap) || (null == m_MinHeap) )
        {
            return;
        }

        BinomialTreeNode<PriorityQueueTuple> removedNode = m_MinHeap.Pop();

        //-- Remove the node from the max heap
        m_MaxHeap.RemoveNode( removedNode.Counterpart );
    }

    public void DequeueMax()
    {
        if( (null == m_MinHeap) || (null == m_MinHeap) )
        {
            return;
        }

        BinomialTreeNode<PriorityQueueTuple> removedNode = m_MaxHeap.Pop();

        //-- Remove the node from the min heap
        m_MinHeap.RemoveNode( removedNode.Counterpart );
    }

    public void Print()
    {
        Console.Out.WriteLine( "Min Heap:" );
        m_MinHeap.Print();
        Console.Out.WriteLine();

        Console.Out.WriteLine( "Max Heap:" );
        m_MaxHeap.Print();
        Console.Out.WriteLine();
    }
}
