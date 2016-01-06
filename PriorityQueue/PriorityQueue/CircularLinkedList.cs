using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Basic doubly linked list node.
/// </summary>
public class CircularLinkedListNode<T>
{
    public CircularLinkedListNode<T> next { get { return m_Next; } set { m_Next = value; } }
    public CircularLinkedListNode<T> previous { get { return m_Previous; } set { m_Previous = value; } }
    public T value { get { return m_Value; } }

    private CircularLinkedListNode<T> m_Next = null;
    private CircularLinkedListNode<T> m_Previous = null;

    private T m_Value;

    public CircularLinkedListNode( T value )
    {
        m_Value = value;
    }
}

/// <summary>
/// A barebones implementation of a circular doubly-linked list.
/// </summary>
public class CircularLinkedList<T>
{
    public int count { get { return m_Count; } }
    public CircularLinkedListNode<T> head { get { return m_Head; } }

    private int m_Count = 0;
    private CircularLinkedListNode<T> m_Head;

    public void Append( T value )
    {
        Append( new CircularLinkedListNode<T>( value ) );
    }

    public void Append( CircularLinkedListNode<T> node )
    {
        if( null == node )
        {
            return;
        }

        if( 0 == m_Count )
        {
            //-- One-node list; trivial
            m_Head = node;
            m_Head.next = m_Head;
            m_Head.previous = m_Head;
        }
        else
        {
            //-- Append the node to the tail
            node.previous = m_Head.previous;
            node.next = m_Head;
            m_Head.previous = node;
        }

        ++m_Count;
    }

    public void Remove( int index )
    {
        if( (0 > index) || (index >= m_Count) )
        {
            //-- Index out of bounds
            return;
        }

        //-- Seek to the node that needs to be removed
        CircularLinkedListNode<T> targetNode = m_Head;

        if( 0 == index )
        {
            //-- Move the head if we need to remove that node
            m_Head = m_Head.next;
        }
        else
        {
            while( 0 < index )
            {
                targetNode = targetNode.next;
                --index;
            }
        }

        //-- Remove the target node
        if( null != targetNode )
        {
            //-- Detach the node from the list
            targetNode.previous.next = targetNode.next;
            targetNode.next.previous = targetNode.previous;
            targetNode.previous = null;
            targetNode.next = null;

            //-- We removed a node, reduce the count
            --m_Count;
        }
    }
}
