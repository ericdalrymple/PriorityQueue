using System;
using System.Text;

class InteractiveInterface
{
    private enum InteractiveActions
    {
          QUIT = 0
        , ENQUEUE
        , DEQUEUE_MIN
        , DEQUEUE_MAX
        , AUTO_POPULATE
    }

    private static PriorityQueue<string> s_Queue = null;
    private static Random s_Rand = null;
    private static StringBuilder s_StringBuffer = null;

    public static void Start( string[] args )
    {
        //-- Initialize static members
        s_Queue = new PriorityQueue<string>();
        s_Rand = new Random();
        s_StringBuffer = new StringBuilder();

        //-- Start interaction loop
        InteractiveActions action;
        do
        {
            //-- Query next action
            action = AskOperation();
            Console.Out.WriteLine( action + " selected.\n\n\n\n" );

            //-- Perform action
            switch( action )
            {
                case InteractiveActions.ENQUEUE:
                {
                    ActionEnqueue();
                    break;
                }

                case InteractiveActions.DEQUEUE_MIN:
                {
                    ActionDequeueMin();
                    break;
                }

                case InteractiveActions.DEQUEUE_MAX:
                {
                    ActionDequeueMax();
                    break;
                }

                case InteractiveActions.AUTO_POPULATE:
                {
                    ActionAutoPopulate();
                    break;
                }
            }

            Console.Out.WriteLine( "\n\n\n\n" );
        }
        while( action != InteractiveActions.QUIT );
    }

    private static InteractiveActions AskOperation()
    {
        Console.Out.WriteLine( "Which action would you like to perform?\n" );

        //-- Get action names
        string[] actionNames = Enum.GetNames( typeof( InteractiveActions ) );

        //-- Enumerate actions for the user
        for( int i = 0; i < actionNames.Length; ++i )
        {
            Console.Out.WriteLine( " " + i + " - " + actionNames[i] );
        }
        Console.Out.WriteLine();

        //-- Query input
        string input = Console.In.ReadLine();

        //-- Match the user input to an action
        try
        {
            int actionId = Int32.Parse( input );
            if( actionId < actionNames.Length )
            {
                return (InteractiveActions)actionId;
            }
        }
        catch( FormatException fe ){}

        Console.Out.WriteLine( "The specified action is not valid, please try again.\n" );

        return AskOperation();
    }

    private static void ActionEnqueue()
    {
        //-- Ask item name
        string name = AskItemValue();
        Console.Out.WriteLine();

        //-- Ask item priority
        int priority = AskItemPriority();
        Console.Out.WriteLine();

        //-- Perform action and print result
        s_Queue.Enqueue( name, priority );
        s_Queue.Print();
    }

    private static void ActionDequeueMin()
    {
        //-- Perform action and print result
        s_Queue.DequeueMin();
        s_Queue.Print();
    }

    private static void ActionDequeueMax()
    {
        //-- Perform action and print result
        s_Queue.DequeueMax();
        s_Queue.Print();
    }

    private static void ActionAutoPopulate()
    {
        //-- Ask item priority
        int count = AskItemCount();
        Console.Out.WriteLine();

        //-- Perform action and print result
        s_Queue = new PriorityQueue<string>();
        for( int i = 0; i < count; ++i )
        {
            s_Queue.Enqueue( GenerateValue( i ), s_Rand.Next( 0, count ) );
        }
        s_Queue.Print();
    }

    private static string AskItemValue()
    {
        Console.Out.WriteLine( "Enter a value: " );
        return Console.In.ReadLine();
    }

    private static int AskItemPriority()
    {
        Console.Out.WriteLine( "Enter a priority: " );
        string priorityString = Console.In.ReadLine();

        try
        {
            return Int32.Parse( priorityString );
        }
        catch( FormatException fe )
        {
            return AskItemPriority();
        }
    }

    private static int AskItemCount()
    {
        Console.Out.WriteLine( "Number of items: " );
        string countString = Console.In.ReadLine();

        try
        {
            return Int32.Parse( countString );
        }
        catch( FormatException fe )
        {
            return AskItemCount();
        }
    }

    /// <summary>
    /// Converts the the specified id to a base-26 value where each digit is a
    /// lower-case letter from the alphabet.
    /// </summary>
    /// <param name="id">Number to use in order to generate the string name.</param>
    /// <returns>A string value.</returns>
    private static string GenerateValue( int id )
    {
        s_StringBuffer.Length = 0;

        do
        {
            s_StringBuffer.Append( (char)('a' + (id % 26)) );
            id /= 26;
        }
        while( id > 0 );

        return s_StringBuffer.ToString();
    }
}
