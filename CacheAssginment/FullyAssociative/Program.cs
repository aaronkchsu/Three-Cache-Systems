using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullyAssociative
{
    class Program
    {
        static void Main(string[] args)
        {
            FullyAssociative(16, 8); // blocksize to rows
        }

        public static void FullyAssociative(int blocksize, int numberofrows){
            int[] addresses = { 16, 20, 24, 28, 32, 36, 60, 64, 56, 60, 64, 68, 72, 76, 92, 96, 100, 104, 108, 112, 136, 140 };
            int Blocksize = blocksize; // Bytes
            int NumberOfRows = numberofrows;
            int loop = 1;
            int hitCount = 0;
            int missCount = 0;

            Queue<int> tags = new Queue<int>(); // A cache where we queue and enque in ehre
            for (int i = 0; i < loop; i++)
            { // Decides how many times we loop it through this
                foreach (int s in addresses)
                {
                    int RowIndex = (s / Blocksize) % NumberOfRows;
                    int CurrentTag = s / Blocksize;
                    if (tags.Contains(CurrentTag))
                    {
                        Console.WriteLine(s + "Hit!!");
                        hitCount++;
                    }
                    else
                    {
                        if (tags.Count > NumberOfRows)
                        {
                            tags.Dequeue(); // Dequeues the last entered queue
                        }
                        Console.WriteLine(s + "MISS!!!");
                        missCount++;
                        tags.Enqueue(CurrentTag); // Enqueues the new tag
                    }
                }
                Console.WriteLine("=============================== ");
            }
            Console.WriteLine("MissCount: " + missCount + " HitCount: " + hitCount);
            int AverageCPI = (((missCount + Blocksize) * 18) + hitCount) / addresses.Length;
            Console.WriteLine("The average CPI is " + AverageCPI);
            Console.Read();
    }
    }
}
