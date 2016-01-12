using System;

/// <summary>
/// An implementation of a priority queue deliberately made from scratch.
/// Implemented in the form of two Binomial Heaps sorted in opposite orders. This
/// was done to have DequeueMin and DequeueMax be equally efficient. The elements
/// from both Binary Heaps match 1-to-1 and matching nodes from both heaps have
/// a reference to each other. When an item gets dequeued from one heap, its match
/// can be removed from the other heap efficiently because we fon't need to dig
/// for the node.
/// </summary>
/// <typeparam name="T">The element class contained by this priority queue</typeparam>
class PriorityQueue<T>
{
    /// <summary>
    /// Sorting order for the queue tuples. This is the backbone of the
    /// two inversely sorted internal heaps.
    /// </summary>
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
        public PriorityQueueTuple( SortMode mode, T value, int priority )
        {
            m_Value = value;
            m_Priority = priority;
            m_SortMode = mode;
        }

        /// <summary>
        /// Compares one tuple against another according to its sort mode.
        /// </summary>
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

        /// <summary>
        /// Returns a display string for this tuple.
        /// </summary>
        /// <returns>A display string.</returns>
        public string ToVerboseString()
        {
            String suffix = (SortMode.ASCENDING == m_SortMode) ? "a" : "d";
            return "(" + m_Value.ToString() + ", " + m_Priority + ")" + suffix;
        }
    }

    private BinomialHeap<PriorityQueueTuple> m_MinHeap = null;
    private BinomialHeap<PriorityQueueTuple> m_MaxHeap = null;

    /// <summary>
    /// The value in the queue with the highest priority.
    /// </summary>
    public T Max
    {
        get
        {
            //-- Skim the top of the hi-to-lo heap
            PriorityQueueTuple result = m_MaxHeap.Peek();
            if( null != result )
            {
                return result.Value;
            }

            return default( T );
        }
    }

    /// <summary>
    /// The value in the queue with the lowest priority.
    /// </summary>
    public T Min
    {
        get
        {
            //-- Skim the top of the lo-to-hi heap
            PriorityQueueTuple result = m_MinHeap.Peek();
            if( null != result )
            {
                return result.Value;
            }

            return default( T );
        }
    }

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
        //-- Create our inversely sorted tuples
        PriorityQueueTuple newMinEntry = new PriorityQueueTuple( SortMode.ASCENDING, value, priority );
        PriorityQueueTuple newMaxEntry = new PriorityQueueTuple( SortMode.DESCENDING, value, priority );

        //-- Add to the min heap
        BinomialHeap<PriorityQueueTuple> newMinHeap = new BinomialHeap<PriorityQueueTuple>( newMinEntry );
        
        //-- Add to the max heap
        BinomialHeap<PriorityQueueTuple> newMaxHeap = new BinomialHeap<PriorityQueueTuple>( newMaxEntry );

        //-- Link the new nodes together so we can access them from each other
        newMinHeap.TreeList.First.Value.Root.m_Counterpart = newMaxHeap.TreeList.First.Value.Root;
        newMaxHeap.TreeList.First.Value.Root.m_Counterpart = newMinHeap.TreeList.First.Value.Root;
        
        //-- Integrate the new size-1 heaps into the existing heaps
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

    /// <summary>
    /// Removes the lowest priority value from this priority queue.
    /// </summary>
    /// <remarks>
    /// Complexity: O(log n)
    /// </remarks>
    public void DequeueMin()
    {
        if( (null == m_MinHeap) || (null == m_MinHeap) )
        {
            return;
        }

        //-- Remove the top node of the lo-to-hi heap
        BinomialTreeNode<PriorityQueueTuple> removedNode = m_MinHeap.Pop();
        if( null == removedNode )
        {
            //-- Heap is empty
            return;
        }
        
        //-- Remove the matching node from the max heap
        m_MaxHeap.RemoveNode( removedNode.Counterpart );
    }

    /// <summary>
    /// Removes the highest priority value from this priority queue.
    /// </summary>
    /// <remarks>
    /// Complexity: O(log n)
    /// </remarks>
    public void DequeueMax()
    {
        if( (null == m_MinHeap) || (null == m_MinHeap) )
        {
            return;
        }

        //-- Remove the top node of the hi-to-lo heap
        BinomialTreeNode<PriorityQueueTuple> removedNode = m_MaxHeap.Pop();
        if( null == removedNode )
        {
            //-- Heap is empty
            return;
        }

        //-- Remove the matching node from the min heap
        m_MinHeap.RemoveNode( removedNode.Counterpart );
    }

    public void Print()
    {
        Console.Out.WriteLine();
        m_MinHeap.Print();
        Console.Out.WriteLine();
    }

    public void PrintVerbose()
    {
        Console.Out.WriteLine( "Min Heap:" );
        m_MinHeap.Print();
        Console.Out.WriteLine();

        Console.Out.WriteLine( "Max Heap:" );
        m_MaxHeap.Print();
        Console.Out.WriteLine();
    }
}
