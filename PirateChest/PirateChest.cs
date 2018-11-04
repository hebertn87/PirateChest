using System;
using System.Collections;
using System.Collections.Generic;

namespace PirateChest
{
    class PirateChest
    {
        public static int chestRows, chestCols, pondRows, pondCols;
        public static long[,] pondMap;

        static void Main(string[] args)
        {

            //Comment this out if you want to use Random Values
            //UserInputMethod();

            //Comment this out if you want to use user input
            RandomValuesMethod();         

            Console.WriteLine("The answer is: " + CalculateMaxChest());
            Console.ReadLine();
           
        }

        //This one takes user input, in order to test the answers of the calculation
        public static void UserInputMethod()
        {
            Console.WriteLine("Input Chest rows: ");
            var maxChestRows = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Input Chest columns: ");
            var maxChestCols = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Input Pond Rows: ");
            pondRows = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Input Pond Columns: ");
            pondCols = Int32.Parse(Console.ReadLine());

            //We only care about the max chest size at first
            chestRows = Math.Min(maxChestRows, maxChestCols);
            chestCols = Math.Max(maxChestRows, maxChestCols);

            pondMap = new long[pondRows, pondCols];

            for (int i = 0; i < pondRows; i++)
            {
                Console.WriteLine("Input the Map of the pond for row " + (i + 1) + ": ");
                for (int j = 0; j < pondCols; j++)
                {

                    pondMap[i, j] = Int32.Parse(Console.ReadLine());
                }
            }
        }

        public static void RandomValuesMethod()
        {
            Random rand = new Random();
            var maxChestRows = rand.Next(1, 1000);
            var maxChestCols = rand.Next(1, 1000);

            pondRows = rand.Next(1, 1000);
            pondCols = rand.Next(1, 1000);

            //We only care about the max chest size at first
            chestRows = Math.Min(maxChestRows, maxChestCols);
            chestCols = Math.Max(maxChestRows, maxChestCols);

            Console.WriteLine("Random Values are as follows: ");
            Console.WriteLine("Chest Rows: " + chestRows);
            Console.WriteLine("Chest Columns: " + chestCols);
            Console.WriteLine("Pond Rows: " + pondRows);
            Console.WriteLine("Pond Columns: " + pondCols);

            pondMap = new long[pondRows, pondCols];

            for (int i = 0; i < pondRows; i++)
            {
                for (int j = 0; j < pondCols; j++)
                {
                    pondMap[i, j] = rand.Next(1, 9999);
                }
            }

        }

        //Method that will calculate the Max chest it can possibly do.
        public static long CalculateMaxChest()
        {
            long best = 0;

            for(int i = 0; i < pondCols; i++)
            {
                //Create a long list of longs
                long[] depthList = new long[pondRows];

                //for each row in pond rows, calculate depths
                for(int k = 0; k < pondRows; k++)
                    depthList[k] = pondMap[k, i];
                
                for(int j = i; j < i+chestRows && j < pondCols; j++)
                {
                    long width = (long)(j - i + 1);
                    var maxDimen = chestRows;

                    if (width <= chestRows)
                        maxDimen = chestCols;

                    for (int k = 0; k < pondRows; k++)
                        depthList[k] = Math.Min(depthList[k], pondMap[k, j]);

                    Stack<StackTuple> s = new Stack<StackTuple>();

                    for(int k = 0; k < pondRows; k++)
                    {
                        if (s.Count == 0 || depthList[k] > s.Peek().depth)
                            s.Push(new StackTuple(k, k, depthList[k]));

                        else
                        {
                            //Make sure we get things that manage to stop here
                            while (s.Count > 0 && depthList[k] <= s.Peek().depth)
                            {
                                StackTuple done = s.Pop();
                                long tryBest = CalcDisplacement(width * Convert.ToInt64(Math.Min(k - done.start, maxDimen)), done.depth);
                                if (tryBest > best)
                                    best = tryBest;
                            }


                            if (s.Count == 0)
                                s.Push(new StackTuple(0, k, depthList[k]));

                            else if (depthList[k] > s.Peek().depth)
                                s.Push(new StackTuple(s.Peek().index + 1, k, depthList[k]));
                        }
                    }

                    //While everything else stops here
                    while(s.Count > 0)
                    {
                        StackTuple done = s.Pop();
                        long tryBest = CalcDisplacement(width * Convert.ToInt64(Math.Min(pondRows - done.start, maxDimen)), done.depth);
                        if (tryBest > best)
                            best = tryBest;
                    }

                }
            }

            return best;
        }

        //Takes a cross area and depths and calculates displacement
        public static long CalcDisplacement(long crossArea, long myDepth)
        {
            long currentVolume = myDepth * crossArea;
            long displace = (currentVolume - 1) / (Convert.ToInt64(pondRows * pondCols) - crossArea);
            currentVolume = (myDepth + displace) * crossArea;

            return currentVolume;
        }

        //Stack tuple class takes a pair of starting values and depths and puts them into a stack up above
        class StackTuple
        {
            public long start;
            public long index;
            public long depth;

            public StackTuple(long _start, long _index, long _depth)
            {
                start = _start;
                index = _index;
                depth = _depth;
            }
        }
    }
}
