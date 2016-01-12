using System;


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

    /// <summary>
    /// Creates a new node having the specified value.
    /// </summary>
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

    public void Print()
    {
        Print( "", true, false );
    }

    public void PrintVerbose()
    {
        Print( "", true, true );
    }

    /// <summary>
    /// Prints a graphical representation of a tree having this node as
    /// its root node.
    /// </summary>
    /// <param name="indent">Padding needed on the left for proper indenting.</param>
    /// <param name="last">Maker for leaf nodes.</param>
    /// <param name="verbose">If true, will print more details for each node.</param>
    private void Print( string indent, bool last, bool verbose )
    {
        //-- Render tree edges
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

        //-- Print the node value
        if( verbose )
        {
            Console.WriteLine( Value + " : " + m_Counterpart );
        }
        else
        {
            Console.WriteLine( Value );
        }

        //-- Iterate on children recursively
        if( Children.IsEmpty )
        {
            return;
        }

        CircularLinkedListNode<BinomialTreeNode<T>> child = Children.First;
        do
        {
            child.Value.Print( indent, (child == Children.Last), verbose );
            child = child.Next;
        }
        while( Children.First != child );
    }
}

/// <summary>
/// A binomial tree is a type of tree that can only be grown by merging
/// with other binomial trees of the same order. Every time two binomial
/// trees are merged, the resulting tree's order is increased by one.
/// 
/// A binomial trees nodes follow the minimum heap rule, which means that
/// a parent node's value is always greater than that of its child nodes.
/// 
/// Binomial trees have insteresting properties that make them efficient to
/// use in the context of a binomial heap algorithm.
/// </summary>
class BinomialTree<T>
where T : IComparable<T>
{
    /// <summary>
    /// Merges two binomial trees together by simply adding the tree whose root
    /// value is largest as a subtree of the other tree's root.
    /// </summary>
    /// <remarks>
    /// Complexity: O(1)
    /// </remarks>
    /// <param name="a">First binomial tree.</param>
    /// <param name="b">Second binomial tree.</param>
    /// <returns>The merged binomial tree.</returns>
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

        //-- If both trees are valid...
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

    /// <summary>
    /// Adds a specified binomial tree of the same order as a
    /// sub-tree of this one by adding its root node as a child of
    /// this tree's root node.
    /// </summary>
    /// <remarks>
    /// Complexity: O(1)
    /// </remarks>
    /// <param name="other">Tree to add.</param>
    /// <returns>The resulting tree (this tree).</returns>
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

        m_Root.Print();
    }

    public void PrintVerbose()
    {
        if( null == m_Root )
        {
            Console.Out.WriteLine( "<EMPTY>" );
            return;
        }

        m_Root.PrintVerbose();
    }
}
