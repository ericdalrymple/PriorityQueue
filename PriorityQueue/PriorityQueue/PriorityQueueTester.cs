using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class PriorityQueueTester
{
    static void Main( string[] args )
    {
        PriorityQueue<string> pq = new PriorityQueue<string>();
        pq.Enqueue( "Hello", 4 );
        pq.Enqueue( "my"   , 1 );
        pq.Enqueue( "name" , 5 );
        pq.Enqueue( "is"   , 3 );
        pq.Enqueue( "John" , 8 );

        Console.Out.WriteLine( pq.min );

        Console.In.ReadLine();
    }
}
