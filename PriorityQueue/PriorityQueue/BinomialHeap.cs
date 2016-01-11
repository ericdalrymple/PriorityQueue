using System;
using System.Collections.Generic;

class BinomialHeap<T>
where T : IComparable<T>
{
    /// <summary>
    /// Combines the trees from two binomial heaps into one list. The trees in the returned
    /// list are ordered by their binomial treee "order" and each tree has an order unique
    /// from the other trees in the list. Trees in the two heaps having the same order are
    /// merged into a single tree whereas one-offs are simply added.
    /// </summary>
    /// <remarks>
    /// Complexity: O(log n)
    /// </remarks>
    /// <param name="a">First binomial heap.</param>
    /// <param name="b">Second binomial heap.</param>
    /// <returns></returns>
    private static LinkedList<BinomialTree<T>> Merge( BinomialHeap<T> a, BinomialHeap<T> b )
    {
        LinkedList<BinomialTree<T>> mergedTrees = new LinkedList<BinomialTree<T>>();

        LinkedListNode<BinomialTree<T>> aIter = a.TreeList.First;
        LinkedListNode<BinomialTree<T>> bIter = b.TreeList.First;

        BinomialTree<T> pendingTree = null;

        while( (null != aIter) || (null != bIter) )
        {
            if( (null == aIter) || (null == bIter) )
            {
                //-- Only one of the trees is good. Store it and nudge
                //   the corresponding iterator.
                if( null != aIter )
                {
                    pendingTree = aIter.Value;
                    aIter = aIter.Next;
                }

                if( null != bIter )
                {
                    pendingTree = bIter.Value;
                    bIter = bIter.Next;
                }
            }
            else
            {
                if( aIter.Value.Order < bIter.Value.Order )
                {
                    //-- Advance A since B is ahead
                    pendingTree = aIter.Value;
                    aIter = aIter.Next;
                }
                else if( aIter.Value.Order > bIter.Value.Order )
                {
                    //-- Advance B since A is ahead
                    pendingTree = bIter.Value;
                    bIter = bIter.Next;
                }
                else
                {
                    //-- A and B have mathing order, merge them and advance both
                    pendingTree = BinomialTree<T>.Merge( aIter.Value, bIter.Value );
                    aIter = aIter.Next;
                    bIter = bIter.Next;
                }
            }

            //-- Try to combine with a list we've already merged if possible
            if( (0 < mergedTrees.Count) && (pendingTree.Order == mergedTrees.Last.Value.Order) )
            {
                pendingTree = BinomialTree<T>.Merge( pendingTree, mergedTrees.Last.Value );
                mergedTrees.RemoveLast();
            }

            //-- Add the new tree to the merged tree list
            mergedTrees.AddLast( pendingTree );
        }

        return mergedTrees;
    }
    
    /// <summary>
    /// List of this heap's binomial trees.
    /// </summary>
    public LinkedList<BinomialTree<T>> TreeList { get { return m_Trees; } }
    
    private LinkedList<BinomialTree<T>> m_Trees = new LinkedList<BinomialTree<T>>();

    /// <summary>
    /// Private constructor used for removing elements from the heap.
    /// </summary>
    private BinomialHeap()
    {
    }

    /// <summary>
    /// Instantiates a new binomial heap containing a single element.
    /// </summary>
    /// <param name="initialValue">Initial element to put into the heap.</param>
    public BinomialHeap( T initialValue )
    {
        m_Trees.AddLast( new BinomialTree<T>( initialValue ) );
    }

    /// <summary>
    /// Returns the value on top of the heap.
    /// </summary>
    /// <remarks>
    /// Complexity: O(1)
    /// </remarks>
    /// <returns></returns>
    public T Peek()
    {
        LinkedListNode<BinomialTree<T>> peekTree = PeekBase();
        if( null != peekTree )
        {
            return peekTree.Value.Root.Value;
        }

        return default( T );
    }

    /// <summary>
    /// Returns the node on top of the heap.
    /// </summary>
    /// <remarks>
    /// Complexity: O(1)
    /// </remarks>
    /// <returns></returns>
    public BinomialTreeNode<T> PeekNode()
    {
        LinkedListNode<BinomialTree<T>> peekTree = PeekBase();
        if( null != peekTree )
        {
            return peekTree.Value.Root;
        }

        return null;
    }

    /// <summary>
    /// Removes the top value from the heap while simultaneously returning it.
    /// </summary>
    /// <remarks>
    /// Complexity: O(log n)
    /// </remarks>
    /// <returns>The top value on the heap that was removed.</returns>
    public BinomialTreeNode<T> Pop()
    {
        //-- Get the tree with the topmost root
        LinkedListNode<BinomialTree<T>> peekTree = PeekBase();
        if( null == peekTree )
        {
            return null;
        }

        RemoveRootNode( peekTree );

        //-- Return the value of the popped node
        return peekTree.Value.Root;
    }

    public void RemoveNode( BinomialTreeNode<T> node )
    {
        BinomialTreeNode<T> parentNode = null;

        //-- Bubble the node to the top of the heap
        while( null != node.Parent )
        {
            parentNode = node.Parent;

            //-- Swap the value with the parent
            T tempValue = parentNode.Value;
            parentNode.m_Value = node.Value;
            node.m_Value = tempValue;

            //-- Swap the counterparts with the parent
            BinomialTreeNode<T> tempCounterpart = parentNode.Counterpart;
            parentNode.m_Counterpart = node.Counterpart;
            node.m_Counterpart = tempCounterpart;

            //-- Move to the parent
            node = parentNode;
        }

        //-- Find the tree the root node belongs to
        LinkedListNode<BinomialTree<T>> treeIter = m_Trees.First;
        while( null != treeIter )
        {
            if( treeIter.Value.Root == node )
            {
                //-- Matching root node found; remove from heap
                RemoveRootNode( treeIter );
                return;
            }

            treeIter = treeIter.Next;
        }
    }

    /// <summary>
    /// Incorporates a specified binomial heap into this binomial heap by
    /// merging the trees from both.
    /// </summary>
    /// <remarks>
    /// Complexity: O(log n)
    /// </remarks>
    /// <param name="other">Binomial heap to union with.</param>
    public void Union( BinomialHeap<T> other )
    {
        LinkedList<BinomialTree<T>> newTreeList = Merge( this, other );
        m_Trees = newTreeList;
    }

    public void Print()
    {
        LinkedListNode<BinomialTree<T>> tree = m_Trees.First;
        while( null != tree )
        {
            tree.Value.Print();
            tree = tree.Next;
        }
    }

    private LinkedListNode<BinomialTree<T>> PeekBase()
    {
        LinkedListNode<BinomialTree<T>> treeIter = m_Trees.First;
        LinkedListNode<BinomialTree<T>> bestFitIter = null;
        while( null != treeIter )
        {
            if( !treeIter.Value.IsEmpty )
            {
                if( null == bestFitIter )
                {
                    //-- Fist value encountered
                    bestFitIter = treeIter;
                }
                else if( bestFitIter.Value.Root.CompareTo( treeIter.Value.Root ) > 0 )
                {
                    //-- New top value
                    bestFitIter = treeIter;
                }
            }

            //-- Iterate
            treeIter = treeIter.Next;
        }

        if( null != bestFitIter )
        {
            return bestFitIter;
        }

        return null;
    }

    private void RemoveRootNode( LinkedListNode<BinomialTree<T>> tree )
    {
        //-- Remove the tree from this heap
        m_Trees.Remove( tree );

        //-- Rejig the heap
        if( 0 < tree.Value.Root.Children.Count )
        {
            //-- Add that tree's sub-trees into a spearate heap
            BinomialHeap<T> temp = new BinomialHeap<T>();
            CircularLinkedListNode<BinomialTreeNode<T>> childIter = tree.Value.Root.Children.First;
            do
            {
                //-- Add the children of the top node as separate trees in the temp
                //   heap from smallest to largest order.
                temp.m_Trees.AddLast( new BinomialTree<T>( childIter.Value ) );
                childIter = childIter.Next;
            }
            while( tree.Value.Root.Children.First != childIter );

            //-- Merge the sub-trees back into this heap
            Union( temp );
        }
    }
}
