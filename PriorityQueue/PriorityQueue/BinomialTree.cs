using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BinomialTreeNode<T>
: IComparable<BinomialTreeNode<T>>
where T : IComparable<T>
{
    public T value { get { return m_Value; } }
    public CircularLinkedList<BinomialTreeNode<T>> children { get { return m_Children; } }

    private T m_Value;
    private CircularLinkedList<BinomialTreeNode<T>> m_Children = new CircularLinkedList<BinomialTreeNode<T>>();

    public BinomialTreeNode( T value )
    {
        m_Value = value;
    }

    public int CompareTo( BinomialTreeNode<T> other )
    {
        return m_Value.CompareTo( other.m_Value );
    }
}

class BinomialTree<T>
where T : IComparable<T>
{
    public static BinomialTree<T> Merge( BinomialTree<T> a, BinomialTree<T> b )
    {
        if( a.isEmpty || b.isEmpty )
        {
            return null;
        }

        if( a.root.CompareTo( b.root ) <= 0 )
        {
            //-- a's root wins, add b to it
            return a.AddSubTree( b );
        }
        else
        {
            //-- b's root wins, add a to it
            return b.AddSubTree( a );
        }
    }

    public bool isEmpty { get { return (m_Root == null); } }
    public int order { get { return m_Order; } }
    public BinomialTreeNode<T> root { get { return m_Root; } }

    private int m_Order = -1;
    private BinomialTreeNode<T> m_Root = null;

    public BinomialTree()
    {
        m_Root = null;
        m_Order = -1;
    }

    public BinomialTree( T rootValue )
    {
        m_Root = new BinomialTreeNode<T>( rootValue );
        m_Order = 0;
    }

    public BinomialTree<T> AddSubTree( BinomialTree<T> other )
    {
        if( m_Order != other.m_Order )
        {
            //-- Only merge binomial trees of the same order
            return null;
        }

        //-- Add the other tree's root as a child of our own
        m_Root.children.Append( other.root );

        //-- Our binomial tree is now one level "deeper"
        ++m_Order;

        return this;
    }
}
