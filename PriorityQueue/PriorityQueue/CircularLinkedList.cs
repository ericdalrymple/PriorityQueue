using System;
using System.Collections.Generic;

/// <summary>
/// Basic doubly linked list node.
/// </summary>
public sealed class CircularLinkedListNode<T>
{
    internal CircularLinkedList<T> m_List = null;
    internal CircularLinkedListNode<T> m_Next = null;
    internal CircularLinkedListNode<T> m_Previous = null;

    /// <summary>
    /// List to which this node belongs.
    /// </summary>
    public CircularLinkedList<T> List { get { return m_List; } }

    /// <summary>
    /// Node that comes after this one in this node's list.
    /// </summary>
    public CircularLinkedListNode<T> Next { get { return m_Next; } }

    /// <summary>
    /// Node that comes before this one in this node's list.
    /// </summary>
    public CircularLinkedListNode<T> Previous { get { return m_Previous; } }

    /// <summary>
    /// This node's value.
    /// </summary>
    public T Value { get; set; }

    internal CircularLinkedListNode( T value )
    {
        Value = value;
    }
    
    public override string ToString()
    {
        return Value.ToString();
    }
}

/// <summary>
/// A barebones implementation of a circular doubly-linked list.
/// </summary>
public sealed class CircularLinkedList<T>
{
    private int m_Count = 0;
    private CircularLinkedListNode<T> m_First;

    /// <summary>
    /// True if this list is empty; false otherwise.
    /// </summary>
    public bool IsEmpty { get { return (null == m_First); } }

    /// <summary>
    /// The number of elements in this list.
    /// </summary>
    public int Count { get { return m_Count; } }

    /// <summary>
    /// The first node of this list.
    /// </summary>
    public CircularLinkedListNode<T> First { get { return m_First; } }

    /// <summary>
    /// The last node of this list.
    /// </summary>
    public CircularLinkedListNode<T> Last { get { return IsEmpty? null : m_First.Previous; } }

    /// <summary>
    /// Appends a specified value to the end of the list and returns the list node
    /// that was created to house it.
    /// </summary>
    /// <remarks>
    /// Complexity: O(1)
    /// </remarks>
    /// <param name="value">Value to be added to this list.</param>
    /// <returns>A new CircularLinkedListNode instance belonging to this list containing the appended value.</returns>
    public CircularLinkedListNode<T> Append( T value )
    {
        CircularLinkedListNode<T> newNode = new CircularLinkedListNode<T>( value );
        Append( newNode );
        return newNode;
    }

    /// <summary>
    /// Searches this list for a node containing a specified value.
    /// </summary>
    /// <remarks>
    /// Complexity: O(n)
    /// </remarks>
    /// <param name="value">Value to look for.</param>
    /// <returns>True if this list contains a node with the specified value; false otherwise.</returns>
    public bool Contains( T value )
    {
        return (null != Find( value ));
    }

    /// <summary>
    /// Searches this list for a node containing a specified value.
    /// </summary>
    /// <remarks>
    /// Complexity: O(n)
    /// </remarks>
    /// <param name="value">Value associated to the node to be removed from the list.</param>
    /// <returns>The CircularLinkedListNode instance that matches the specified value or 'null' if no match was found.</returns>
    public CircularLinkedListNode<T> Find( T value )
    {
        if( IsEmpty )
        {
            return null;
        }
        
        //-- Traverse the list in both directions simultaneously
        CircularLinkedListNode<T> iter = m_First;
        do
        {
            if( EqualityComparer<T>.Equals( iter, value ) )
            {
                //-- Found; stop looking
                return iter;
            }
            
            iter = iter.Next;
        }
        while( iter != m_First );

        return null;
    }

    /// <summary>
    /// Removes the node in the list that comes after a specified node. No node is removed if
    /// the specified node does not belong to this list or if this list is empty.
    /// </summary>
    /// <remarks>
    /// Complexity: O(1)
    /// </remarks>
    /// <param name="node">Node belonging to this list preceeding the node to remove.</param>
    /// <returns>The node that was removed from the list or 'null' if no node was removed.</returns>
    public CircularLinkedListNode<T> RemoveAfter( CircularLinkedListNode<T> node )
    {
        return RemoveNodeBase( node.Next );
    }

    /// <summary>
    /// Removes the list element at the specified index from this list. No node is removed
    /// if the list is empty or if the index is out of bounds.
    /// </summary>
    /// <remarks>
    /// Complexity: O(n)
    /// </remarks>
    /// <param name="index">0-based index marking the node's position.</param>
    /// <returns>The node that was removed from the list or 'null' if no node was removed.</returns>
    public CircularLinkedListNode<T> RemoveAt( int index )
    {
        if( (0 > index) || (index >= m_Count) || IsEmpty )
        {
            //-- Index out of bounds
            return null;
        }

        //-- Seek to the node that needs to be removed
        CircularLinkedListNode<T> removedNode = m_First;
        while( 0 < index )
        {
            removedNode = removedNode.Next;
            --index;
        }

        //-- Remove the target node
        return RemoveNodeBase( removedNode );
    }

    public void Print()
    {
        Console.Out.Write( "[" );

        CircularLinkedListNode<T> node = First;
        if( null != node )
        {
            do
            {
                Console.Out.Write( node.Value.ToString() );

                node = node.Next;
                if( First != node )
                {
                    Console.Out.Write( ", " );
                }
            }
            while( First != node );
        }

        Console.Out.WriteLine( "]" );
        Console.Out.WriteLine( "Size: " + Count );
    }

    private bool Append( CircularLinkedListNode<T> node )
    {
        if( (null == node) || (null != node.m_List) )
        {
            //-- Node is null or already belongs to another list
            return false;
        }

        //-- Set self as list
        node.m_List = this;

        if( 0 == m_Count )
        {
            //-- One-node list; trivial
            m_First = node;
            m_First.m_Next = m_First;
            m_First.m_Previous = m_First;
        }
        else
        {
            //-- Append the node to the tail
            node.m_Previous = m_First.Previous;
            m_First.m_Previous.m_Next = node;

            //-- Prepend the node to the head
            node.m_Next = m_First;
            m_First.m_Previous = node;
        }

        ++m_Count;

        return true;
    }

    private CircularLinkedListNode<T> RemoveNodeBase( CircularLinkedListNode<T> node )
    {
        //-- Ensure the node is valid and belongs to this list
        if( (null != node) && (this == node.List) )
        {
            //-- Safeguard the head reference if that's the node we're removing
            if( node == m_First )
            {
                if( m_First == m_First.Next )
                {
                    //-- We are removing the last element; null the reference to the first element
                    m_First = null;
                }
                else
                {
                    //-- Change the second node to be the first node
                    m_First = m_First.Next;
                }
            }

            //-- Detach the node from the list
            node.m_Previous.m_Next = node.Next;
            node.m_Next.m_Previous = node.Previous;
            node.m_Previous = null;
            node.m_Next = null;
            node.m_List = null;

            //-- We removed a node, reduce the count
            --m_Count;

            //-- Return removed node value
            return node;
        }

        return null;
    }
}
