using System;
using System.Collections.Generic;
using System.Text;

public class BinomialTreeNode<T>
: IComparable<BinomialTreeNode<T>>
where T : IComparable<T>
{
    internal BinomialTreeNode<T> m_Counterpart = null;
    internal BinomialTreeNode<T> m_Parent = null;
    internal CircularLinkedList<BinomialTreeNode<T>> m_Children = new CircularLinkedList<BinomialTreeNode<T>>();
    internal T m_Value = default( T );

    /// <summary>
    /// This node's stored value.
    /// </summary>
    public T Value { get { return m_Value; } }

    /// <summary>
    /// A node representing the same element instance as this one. Used for bi-directional heaps.
    /// </summary>
    public BinomialTreeNode<T> Counterpart { get { return m_Counterpart; } }

    /// <summary>
    /// This node's parent node.
    /// </summary>
    public BinomialTreeNode<T> Parent { get { return m_Parent; } }

    /// <summary>
    /// A list of this node's children.
    /// </summary>
    public CircularLinkedList<BinomialTreeNode<T>> Children { get { return m_Children; } }

    internal BinomialTreeNode( T value )
    {
        m_Value = value;
    }

    public int CompareTo( BinomialTreeNode<T> other )
    {
        return m_Value.CompareTo( other.m_Value );
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public void Print( string indent, bool last )
    {
        Console.Write( indent );
        if( last )
        {
            Console.Write( "\\-" );
            indent += "  ";
        }
        else
        {
            Console.Write( "|-" );
            indent += "| ";
        }
        Console.WriteLine( Value );

        if( Children.IsEmpty )
        {
            return;
        }

        CircularLinkedListNode<BinomialTreeNode<T>> child = Children.First;
        do
        {
            child.Value.Print( indent, (child == Children.Last) );
            child = child.Next;
        }
        while( Children.First != child );
    }
}

class BinomialTree<T>
where T : IComparable<T>
{
    public static BinomialTree<T> Merge( BinomialTree<T> a, BinomialTree<T> b )
    {
        BinomialTree<T> mergedTree = new BinomialTree<T>();

        if( (null == a) || (null == b) )
        {
            //-- If one of the trees is null, attempt to return the one
            //   that isn't null if any
            if( null != a )
            {
                mergedTree.AddSubTree( a );
            }
            else if( null != b )
            {
                mergedTree.AddSubTree( b );
            }

            return mergedTree;
        }

        if( a.Root.CompareTo( b.Root ) <= 0 )
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

    private int m_Order = -1;
    private BinomialTreeNode<T> m_Root = null;

    /// <summary>
    /// True if this tree contains nodes; false otherwise.
    /// </summary>
    public bool IsEmpty { get { return (null == m_Root); } }

    /// <summary>
    /// Order of this binomial tree. The order is equivalent to the tree's
    /// number of layers minus one, which is equivalent to the number of binomial
    /// trees that have been merged into this one.
    /// </summary>
    public int Order { get { return m_Order; } }

    /// <summary>
    /// Root node of this tree.
    /// </summary>
    public BinomialTreeNode<T> Root { get { return m_Root; } }

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

    public BinomialTree( BinomialTreeNode<T> rootNode )
    {
        m_Root = rootNode;
        m_Order = 0;
    }

    public BinomialTree<T> AddSubTree( BinomialTree<T> other )
    {
        if( m_Order != other.m_Order )
        {
            //-- Only merge binomial trees of the same order
            return this;
        }

        //-- Add the other tree's root as a child of our own
        if( null != m_Root )
        {
            other.Root.m_Parent = m_Root;
            m_Root.Children.Append( other.Root );
        }
        else
        {
            m_Root = other.Root;
        }

        //-- Our binomial tree is now one level "deeper"
        ++m_Order;

        return this;
    }

    public void Print()
    {
        if( null == m_Root )
        {
            Console.Out.WriteLine( "<EMPTY>" );
            return;
        }

        m_Root.Print( "", true );
    }
}
