using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheTest
{
    class Program
    {

        static void Main(string[] args)
        {
           //DirectMapped(32, 4, 10);
           //FullyAssociative(256, 2, 10);
           SetAssociative(64, 4, 20, 2);
        }

        /// <summary>
        /// A fully associative cache must be stored into list where that lists is updated like a queue
        /// where the data shifts as it goes through
        /// </summary>
        /// <param name="blocksize"></param>
        /// <param name="numberofrows"></param>
        /// <param name="iterations"></param>
        public static void FullyAssociative(int blocksize, int numberofrows, int iterations)
        {
            int[] addresses = { 16, 20, 24, 28, 32, 36, 60, 64, 56, 60, 64, 68, 72, 76, 92, 96, 100, 104, 108, 112, 136, 140 };
            int Blocksize = blocksize / 8; // We want it in bytes
            int NumberOfRows = numberofrows;
            int loop = iterations;
            double hitCount = 0;
            double missCount = 0;
            List<int> tags = new List<int>(); // A cache where we queue and enque in ehre
            for (int i = 0; i < loop; i++)
            { // Decides how many times we loop it through this
                foreach (int s in addresses)
                {
                    int CurrentTag = s / Blocksize;
                    if (tags.Contains(CurrentTag))
                    {
                        tags.Remove(CurrentTag);
                        tags.Add(CurrentTag);
                        System.Diagnostics.Debug.WriteLine(s + " Hit!! " + CurrentTag + " ROW: " + tags.Count);
                        hitCount++;
                    }
                    else
                    {
                        if (tags.Count >= NumberOfRows)
                        {
                            tags.RemoveAt(0); // Dequeues the last entered queue
 
                        }
                        tags.Add(CurrentTag); // Enqueues the new tag
                        System.Diagnostics.Debug.WriteLine(s + " MISS!!! " + CurrentTag + " ROW: " + tags.Count);
                        missCount++;
                    }
                }
                double AverageCPI = (missCount * (18 + Blocksize) + hitCount) / addresses.Length;
                System.Diagnostics.Debug.WriteLine("The average CPI is " + AverageCPI);
                System.Diagnostics.Debug.WriteLine("MissCount: " + missCount + " HitCount: " + hitCount);
                hitCount = 0;
                missCount = 0;
                System.Diagnostics.Debug.WriteLine("=============================== ");
            }
            System.Diagnostics.Debug.WriteLine("FULLY ASSOCIATIVE CACHE");
        }

        /// <summary>
        /// This cache is used for direct mapping cache where each row is read into being mapped in
        /// </summary>
        /// <param name="blocksize"></param>
        /// <param name="numberofrows"></param>
        /// <param name="iterations"></param>
        public static void DirectMapped(int blocksize, int numberofrows, int iterations)
        {
            System.Diagnostics.Debug.WriteLine("START OF PROGRAM ======>");
            int[] addresses = { 16, 20, 24, 28, 32, 36, 60, 64, 56, 60, 64, 68, 72, 76, 92, 96, 100, 104, 108, 112, 136, 140 };
            int Blocksize = blocksize / 8; // We want it in bytes
            int NumberOfRows = numberofrows;
            int loop = iterations;
            double hitCount = 0;
            double missCount = 0;
            int[] tags = new int[NumberOfRows]; // Cache size should be the length of number of rows
            // Invalidate everything!
            for (int i = 0; i < tags.Length; i++)
            {
                tags[i] = -100;
            }
            for (int i = 0; i < loop; i++)
            { // Decides how many times we loop it through this
                foreach (int s in addresses)
                {
                    int RowIndex = (s / Blocksize) % NumberOfRows;
                    int CurrentTag = s / Blocksize / NumberOfRows;
                    if (tags[RowIndex] == CurrentTag)
                    {
                        System.Diagnostics.Debug.WriteLine(s + "Hit!! " + "ROW INDEX:" + RowIndex + " CURRENT TAG:" + CurrentTag);
                        hitCount++;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(s + "MISS!!! " + "ROW INDEX: " + RowIndex + " CURRENT TAG: " + CurrentTag);
                        missCount++;
                        tags[RowIndex] = CurrentTag; // Change the tag
                    }
                }
                System.Diagnostics.Debug.WriteLine("MissCount: " + missCount + " HitCount: " + hitCount);
                double AverageCPI = (missCount * (18 + Blocksize) + hitCount) / addresses.Length;
                System.Diagnostics.Debug.WriteLine("The average CPI is " + AverageCPI);
                hitCount = 0; // Reset
                missCount = 0;
                System.Diagnostics.Debug.WriteLine("=============================== ");
            }
            System.Diagnostics.Debug.WriteLine("DIRECT MAPPED CACHE");
        }

        /// <summary>
        /// A Set associative cache similuation
        /// </summary>
        /// <param name="blocksize"></param>
        /// <param name="numberofrows"></param>
        /// <param name="iterations"></param>
        /// <param name="setAssociation"></param>
        public static void SetAssociative(int blocksize, int numberofrows, int iterations, int setAssociation)
        {
            int[] addresses = { 16, 20, 24, 28, 32, 36, 60, 64, 56, 60, 64, 68, 72, 76, 92, 96, 100, 104, 108, 112, 136, 140 };
            int Blocksize = blocksize / 8; // We want it in bytes
            int NumberOfRows = numberofrows;
            int loop = iterations;
            int setNumber = setAssociation; // n-association block
            double hitCount = 0;
            double missCount = 0;
            List<int>[] CacheRows = new List<int>[NumberOfRows]; // Cache size should be the length of number of rows
            //Fill up the cache rows
            for (int i = 0; i < CacheRows.Length; i++)
            {
                List<int> tags = new List<int>(); // Keeps tags of
                CacheRows[i] = tags;
            }
            for (int i = 0; i < loop; i++)
            { // Decides how many times we loop it through this
                foreach (int s in addresses)
                {
                    int RowIndex = (s / Blocksize) % NumberOfRows;
                    int CurrentTag = s / Blocksize / NumberOfRows;
                    List<int> temp = CacheRows[RowIndex];
                    if (CacheRows[RowIndex].Contains(CurrentTag)) // If the List inside the Cache row has the Current tag then do this
                    {
                        CacheRows[RowIndex].Remove(CurrentTag);
                        CacheRows[RowIndex].Add(CurrentTag); // Reset this tag
                        System.Diagnostics.Debug.WriteLine(s + "Hit!!" + " TAG: " + CurrentTag + " Row Index: " + RowIndex);
                        hitCount++;
                    }
                    else // If it misses
                    {
                        if (CacheRows[RowIndex].Count >= setAssociation) // We have to deque if it is greater then the set association amount
                        {
                            CacheRows[RowIndex].RemoveAt(0); // Dequeues the last entered queue
                        }
                        System.Diagnostics.Debug.WriteLine(s + "MISS!!!" + " TAG: " + CurrentTag + " Row Index: " + RowIndex);
                        missCount++;
                        CacheRows[RowIndex].Add(CurrentTag); // Enqueues the new tag
                    }

                }
                System.Diagnostics.Debug.WriteLine("MissCount: " + missCount + " HitCount: " + hitCount);
                double AverageCPI = (missCount * (18 + Blocksize) + hitCount) / addresses.Length;
                System.Diagnostics.Debug.WriteLine("The average CPI is " + AverageCPI);
                missCount = 0;
                hitCount = 0; // Reset
                System.Diagnostics.Debug.WriteLine("=============================== ");
            }
            System.Diagnostics.Debug.WriteLine(setAssociation + " SET - ASSOCIATIVE CACHE");
        }
    }
}
