# PriorityQueue
A basic priority queue implementation written in C# in the format of a Microsoft Visual Studio 2015 project.

== OVERVIEW ==

This is an implementation of a priority queue based off of two inversely sorted binomial heaps with linked
nodes to avoid seeking.

Please refer to the file guide below for an explanation of the files in the project. Please refer to the in-code
documentation for information on the specifics of the implementation.

Here is a brief rundown of the costs of the test operations:

  Enqueue: O(1)
  DequeueMin: O(log n)
  DequeueMax: O(log n)



== USAGE ==

There is a 'Main' function in the 'PriorityQueueTester' class that will allow you to run an interactive
console demo. The demo allows you to manipulate a priority queue using the operations mentioned in the
test's parameters (enqueue, dequeue min, dequeue max). There is also an option for auto populating a new
queue with any number of nodes with random priorities.

The priority queue resulting from each operation will be displayed every time an operation is performed.



== FILE GUIDE ==

Here is a list of all the source files in the project along with a brief description of their contents. I've
added the complexity of the functions for which it was relevant in the header comments for those functions.

> PriorityQueue.cs
    This might be the class you would want to look at first. It is the priority queue implementation that
    is created and manipulated by the interactive demo.

> PriorityQueueTester.cs
    Main entry point into the interactive console demo. This class is mostly filled with print outs that
    I was using for testing purposes (currently disabled in lieu of the demo).

> InteractiveInterface.cs
    The file that houses all of the logic, flow and input/output for the demo.

> BinomialHeap.cs
    I implemented the priority queue using two inversely sorted instances of binomial heaps. The binomial
    heap code is housed in this file. This file contains additional information about the ins and outs
    of binomial heaps.

> BinomialTree.cs
    Binomial heaps house small sets of binomial trees. The binomial tree code is housed in this file. This
    file contains additional information about the ins and outs of binomial trees.

> CircularLinkedList.cs
    I've opted to use a circular linked list for a few things in the project. This file houses a rudimentary
    circular linked list implementation.
