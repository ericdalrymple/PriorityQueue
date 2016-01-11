using System;
using System.Collections.Generic;
using System.Linq;

class PriorityQueueTester
{
    private class CustomComparer<T>
    : IComparer<T>
    where T : IComparable<T>
    {
        public int Compare( T a, T b )
        {
            int result = a.CompareTo( b );
            if( 0 == result )
            {
                return 1;
            }

            return result;
        }
    }

    static Random rand = new Random();

    static void Main( string[] args )
    {
        TestCircularList();

        Console.Out.WriteLine();

        TestPriorityQueueTuple();

        Console.Out.WriteLine();

        TestBinomialTree();

        Console.Out.WriteLine();

        TestBinomialHeap();

        Console.Out.WriteLine();

        TestPriorityQueue();

        Console.Out.WriteLine( "<Press Return Key to Exit>" );
        Console.In.ReadLine();
    }

    static void TestCircularList()
    {
        string[] items = { "a", "b", "c", "d" };

        Console.Out.WriteLine( "CIRCULAR LINKED LIST TESTS" );
        Console.Out.WriteLine();

        Console.Out.WriteLine( "> Create new list" );
        CircularLinkedList<string> list = new CircularLinkedList<string>();
        list.Print();
        Console.Out.WriteLine();
        
        foreach( string item in items )
        {
            Console.Out.WriteLine( "> Append '" + item + "'" );
            list.Append( item );
            list.Print();
            Console.Out.WriteLine();
        }
        
        Random rand = new Random();
        while( 0 < list.Count )
        {
            int index = rand.Next( 0, list.Count );
            Console.Out.WriteLine( "> Remove at " + index );
            list.RemoveAt( index );
            list.Print();
            Console.Out.WriteLine();
        }
        Console.Out.WriteLine();
    }

    static void TestPriorityQueueTuple()
    {
        Console.Out.WriteLine( "PRIORITY QUEUE TUPLES" );
        Console.Out.WriteLine();

        Console.Out.WriteLine( "== ASCENDING ==" );
        Console.Out.WriteLine();

        TestPriorityQueueTupleSorting( PriorityQueue<string>.SortMode.ASCENDING );

        
        Console.Out.WriteLine( "== DESCENDING ==" );
        Console.Out.WriteLine();
        
        TestPriorityQueueTupleSorting( PriorityQueue<string>.SortMode.DESCENDING );
    }

    static void TestPriorityQueueTupleSorting( PriorityQueue<string>.SortMode sortMode )
    {
        int sortDirection = (PriorityQueue<string>.SortMode.ASCENDING == sortMode) ? 1 : -1;
        string[] items = { "a", "b", "c", "d" };

        //-- Generate a list of tuples with random priorities
        SortedSet<PriorityQueue<string>.PriorityQueueTuple> list = new SortedSet<PriorityQueue<string>.PriorityQueueTuple>( new CustomComparer<PriorityQueue<string>.PriorityQueueTuple>() );
        foreach( string item in items )
        {
            list.Add( new PriorityQueue<string>.PriorityQueueTuple( sortMode
                                                                  , item
                                                                  , rand.Next( 0, 100 ) ) );
        }

        //-- Test if the items are properly ordered according to the specified sort mode and
        //   print them out so the dev can see the order the items were sorted in
        bool pass = true;
        int prevPriority = list.ElementAt( 0 ).Priority;
        foreach( PriorityQueue<string>.PriorityQueueTuple tuple in list )
        {
            Console.Out.WriteLine( tuple.ToString() );
            if( 0 < (prevPriority - tuple.Priority) * sortDirection )
            {
                //-- Priority out of order relative to the previous item
                pass = false;
            }

            prevPriority = tuple.Priority;
        }

        if( pass )
        {
            //-- Items ordered properly
            Console.Out.WriteLine( "PASS!" );
        }
        else
        {
            //-- Items not ordered properly
            Console.Out.WriteLine( "FAIL" );
        }
        Console.Out.WriteLine();
    }

    static void TestBinomialTree()
    {
        Console.Out.WriteLine( "BINOMIAL TREES" );
        Console.Out.WriteLine();

        //-- Create a list of elements
        List<int> elements = new List<int>( new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 } );

        //-- Suffle the elements into an array
        int[] shuffledElements = new int[elements.Count];
        for( int i = 0; i < shuffledElements.Length; ++i )
        {
            int nextElementIndex = rand.Next( 0, elements.Count );
            shuffledElements[i] = elements.ElementAt( nextElementIndex );
            elements.RemoveAt( nextElementIndex );
        }

        //-- Print the shuffled elements
        Console.Out.WriteLine( "Element Set:" );
        foreach( int element in shuffledElements )
        {
            Console.Out.Write( element + " " );
        }
        Console.Out.WriteLine( "\n" );

        //-- Create a large binomial tree with all of the elements
        BinomialTree<int> resultingTree = TestBinomialTree( shuffledElements, 0, shuffledElements.Length - 1 );
        
        //-- Print the result
        Console.Out.WriteLine( "Result:" );
        resultingTree.Print();
        Console.Out.WriteLine();
    }

    static BinomialTree<int> TestBinomialTree( int[] elements, int start, int end )
    {
        if( start == end )
        {
            //-- Create a new tree of size 1
            BinomialTree<int> newTree = new BinomialTree<int>( elements[start] );
            return newTree;
        }

        //-- Combine two sub-trees
        int middle = (start + end) /2;
        BinomialTree<int> intermediateTreeA = TestBinomialTree( elements, start, middle );
        BinomialTree<int> intermediateTreeB = TestBinomialTree( elements, middle + 1, end );
        BinomialTree<int> merger = BinomialTree<int>.Merge( intermediateTreeA, intermediateTreeB );
        return merger;
    }

    static void TestBinomialHeap()
    {
        Console.Out.WriteLine( "BINOMIAL HEAP" );
        Console.Out.WriteLine();

        //-- Create a list of elements
        List<int> elements = new List<int>( new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 } );

        //-- Suffle the elements into an array
        int[] shuffledElements = new int[elements.Count];
        for( int i = 0; i < shuffledElements.Length; ++i )
        {
            int nextElementIndex = rand.Next( 0, elements.Count );
            shuffledElements[i] = elements.ElementAt( nextElementIndex );
            elements.RemoveAt( nextElementIndex );
        }

        //-- Print the shuffled elements
        Console.Out.WriteLine( "Element Set:" );
        foreach( int element in shuffledElements )
        {
            Console.Out.Write( element + " " );
        }
        Console.Out.WriteLine( "\n" );

        //-- Create a large binomial tree with all of the elements
        BinomialHeap<int> resultingHeap = null;
        foreach( int element in shuffledElements )
        {
            BinomialHeap<int> newHeap = new BinomialHeap<int>( element );
            if( null != resultingHeap )
            {
                resultingHeap.Union( newHeap );
            }
            else
            {
                resultingHeap = newHeap;
            }
        }

        //-- Print the result
        Console.Out.WriteLine( "Result: " + resultingHeap.Peek() );
        resultingHeap.Print();
        Console.Out.WriteLine();
    }

    static void TestPriorityQueue()
    {
        Console.Out.WriteLine( "PRIORITY QUEUE" );
        Console.Out.WriteLine();

        PriorityQueue<string> pq = new PriorityQueue<string>();
        pq.Enqueue( "Hello", 4 );
        pq.Enqueue( "my"   , 1 );
        pq.Enqueue( "name" , 5 );
        pq.Enqueue( "is"   , 3 );
        pq.Enqueue( "John" , 8 );
        
        pq.Print();

        pq.DequeueMin();

        pq.Print();
    }
}
