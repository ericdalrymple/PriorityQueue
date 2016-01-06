using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BinomialHeap<T>
where T : IComparable<T>
{
    public static BinomialHeap<T> Merge( BinomialHeap<T> a, BinomialHeap<T> b )
    {
        BinomialHeap<T> mergedHeap = new BinomialHeap<T>();

        CircularLinkedListNode<BinomialTree<T>> aTree = a.treeList.head;
        CircularLinkedListNode<BinomialTree<T>> bTree = b.treeList.head;
        CircularLinkedListNode<BinomialTree<T>> cTree = mergedHeap.treeList.head;

        //-- The trees in each heap are implicitly sorted in increasing
        //   order, and the nature of the algorithm ensures that there
        //   are no gaps, so we can match trees of the same order from both
        //   heaps a and b in one pass without seeking.
        do
        {
            BinomialTree<T> mergedTree = BinomialTree<T>.Merge( aTree.value, bTree.value );

            if( !cTree.value.isEmpty )
            {
                mergedTree = BinomialTree<T>.Merge( cTree.value, mergedTree );
            }

            mergedHeap.AddTree( mergedTree );

            aTree = aTree.next;
            bTree = bTree.next;
            cTree = cTree.next;
        }
        while( (aTree != a.treeList.head) && (bTree != b.treeList.head) );

        return mergedHeap;
    }

    public CircularLinkedList<BinomialTree<T>> treeList { get { return m_Trees; } }
    
    private CircularLinkedList<BinomialTree<T>> m_Trees = new CircularLinkedList<BinomialTree<T>>();

    //-- Don't allow outsiders to create empty heaps; mark private. This helps guarantee the safety of the algorithm.
    private BinomialHeap()
    {
        m_Trees.Append( new BinomialTree<T>() );
    }

    public BinomialHeap( T initialValue )
    {
        m_Trees.Append( new BinomialTree<T>( initialValue ) );
    }

    public T Peek()
    {
        BinomialTreeNode<T> bestFit = null;
        bool first = true;

        CircularLinkedListNode<BinomialTree<T>> treeIter = m_Trees.head;
        do
        {
            if( first )
            {
                bestFit = treeIter.value.root;
                continue;
            }

            if( (null != bestFit) && (bestFit.CompareTo( treeIter.value.root ) < 0) )
            {
                bestFit = treeIter.value.root;
            }
        }
        while( treeIter != m_Trees.head );

        if( null != bestFit )
        {
            return bestFit.value;
        }

        return default( T );
    }

    private void AddTree( BinomialTree<T> tree )
    {
        m_Trees.Append( tree );
    }
}
