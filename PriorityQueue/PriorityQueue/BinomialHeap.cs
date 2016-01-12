using System;
using System.Collections.Generic;

/// <summary>
/// A binomial heap is a structure containing 0 to many binomial trees, each
/// of a unique order. Values are added to it by creating a size 1 binomial
/// heap and merging it with an existing instance.
/// 
/// When two binomial trees are merged, their trees of the same order are merged
/// together and the remaining trees are carried over. Because the merging of binomial
/// trees is in constant time, merging of binomial heaps is also very efficient.
/// 
/// Because binomial trees follow the minimum heap rule, one can get the top value
/// of a binomial heap by returning the lowest value among the roots of its trees.
/// </summary>
class BinomialHeap<T>
where T : IComparable<T>
{
    /// <summary>
    /// Combines the trees from two binomial heaps into one list. The trees in the returned
    /// list are ordered by their binomial treee "order" and each tree has an order unique
    /// from the other trees in the list. Trees in the two heaps having the same order are
    /// merged into a single tree whereas one-offs are simply added to the resulting list.
    /// </summary>
    /// <remarks>
    /// Complexity: O(log n)
    /// </remarks>
    /// <param name="a">First binomial heap.</param>
    /// <param name="b">Second binomial heap.</param>
    /// <returns>A list of merged trees from both input heaps.</returns>
    private static LinkedList<BinomialTree<T>> Merge( BinomialHeap<T> a, BinomialHeap<T> b )
    {
        LinkedList<BinomialTree<T>> mergedTrees = new LinkedList<BinomialTree<T>>();

        LinkedListNode<BinomialTree<T>> aIter = a.TreeList.First;
        LinkedListNode<BinomialTree<T>> bIter = b.TreeList.First;

        BinomialTree<T> pendingTree = null;

        //-- Iterate through the trees of both heaps at once
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

    private LinkedList<BinomialTree<T>> m_Trees = new LinkedList<BinomialTree<T>>();

    /// <summary>
    /// List of this heap's binomial trees.
    /// </summary>
    public LinkedList<BinomialTree<T>> TreeList { get { return m_Trees; } }

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
        //-- Get the internal tree with the topmost root node value
        //   and return that value.
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
        //-- Get the internal tree with the topmost root node value
        //   and return that root node.
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

        //-- Remove the root node from that tree and shuffle
        //   the resulting sub-trees back in.
        RemoveRootNode( peekTree );

        //-- Return the value of the popped node
        return peekTree.Value.Root;
    }

    /// <summary>
    /// Remove the specified node from this heap.
    /// </summary>
    /// <remarks>
    /// Complexity: O(log n)
    /// </remarks>
    /// <param name="node"></param>
    public void RemoveNode( BinomialTreeNode<T> node )
    {
        BinomialTreeNode<T> parentNode = node.Parent;

        //-- Bubble the node to the top of the heap
        while( null != node.Parent )
        {
            //-- Swap the value with the parent
            T tempValue = parentNode.Value;
            parentNode.m_Value = node.Value;
            node.m_Value = tempValue;

            //-- Swap the counterparts with the parent
            node.m_Counterpart.m_Counterpart = parentNode;
            parentNode.m_Counterpart.m_Counterpart = node;

            BinomialTreeNode<T> tempCounterpart = parentNode.Counterpart;
            parentNode.m_Counterpart = node.Counterpart;
            node.m_Counterpart = tempCounterpart;

            //-- Move to the parent
            node = parentNode;
            parentNode = node.Parent;
        }

        //-- Find the tree the root node belongs to
        LinkedListNode<BinomialTree<T>> treeIter = m_Trees.First;
        while( null != treeIter )
        {
            if( treeIter.Value.Root == node )
            {
                //-- Matching root node found; remove from heap
                RemoveRootNode( treeIter );
                break;
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

    public void PrintVerbose()
    {
        LinkedListNode<BinomialTree<T>> tree = m_Trees.First;
        while( null != tree )
        {
            tree.Value.PrintVerbose();
            tree = tree.Next;
        }
    }

    /// <summary>
    /// Loops through this heap's trees and returns an iterator to the
    /// one with the topmost root value.
    /// </summary>
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

    /// <summary>
    /// Removes the root node from the specified internal tree and shuffles
    /// the resulting sub-trees back into this heap.
    /// </summary>
    /// <remarks>
    /// Complexity: O(log n)
    /// </remarks>
    /// <param name="tree">Iterater to the tree who's root we want to remove.</param>
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
                //-- Orphan the child
                childIter.Value.m_Parent = null;

                //-- Add the children of the top node as separate trees in the temp
                //   heap from smallest to largest order.
                temp.m_Trees.AddLast( new BinomialTree<T>( childIter.Value ) );
                childIter = childIter.Next;
            }
            while( tree.Value.Root.Children.First != childIter );

            //-- Merge the resulting sub-trees back into this heap
            Union( temp );
        }
    }
}
