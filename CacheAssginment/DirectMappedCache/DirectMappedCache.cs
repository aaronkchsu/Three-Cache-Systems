using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectMappedCache
{

    /// <summary>
    /// We will be calculating everything in bytes
    /// </summary>
    public class DirectMappedCache
    {

        Dictionary<int, int[]> tagToBlock; // Stores all the values of the substring
        Dictionary<int, int[]>[] cache;
        int blocksize; // This will be in bits
        int rows; // # of rows in numbers
        const int totalsize = 900;

        public DirectMappedCache(int rows_, int blocksize_)
        {
            cache = new Dictionary<int, int[]>[rows_]; // Creates a cache with the length of the number of rows
            tagToBlock = new Dictionary<int, int[]>(); // Key maps to a block
            rows = rows_; // Set the rows 
            blocksize = blocksize_; // Set the blocksize
        }

        /// <summary>
        /// This stores data to the right row 
        /// </summary>
        /// <param name="address">An </param>
        public void StoreData(int address) { 
            int rowIndex = (address / blocksize) % rows; // The row of the cache is determined from
            int tag = address % (blocksize * rows); // The tag is just the block size
            int offset = address % blocksize; // The offset just becomes the mod of the address because it is the first bits
            if(cache[rowIndex].ContainsKey(tag)){ // If tag exists in the data then no need to store
                cache[rowIndex][tag][offset] = tag; // If it exists then add it like this
            }
            else{
                int[] blocks = new int[blocksize / 32]; // As many words are allocated for the blocks - words are 32 bits which 
                blocks[offset] = tag; // Stores tag at offset to occupy the spot
                cache[rowIndex].Add(tag, blocks);
            }
         }



        /// <summary>
        /// Reads data Returns true or false depending on if it hits the cache or misses it
        /// </summary>
        /// <param name="address"></param>
        public Boolean HitOrMiss(int address)
        {
            int rowIndex = (address / blocksize) % rows; // The row of the cache is determined from
            int tag = address % (blocksize * rows); // The tag is decided by the blocksize and the
            int offset = address % blocksize; // The offset just becomes the mod of the address because it is the first bits
            if (cache[rowIndex].ContainsKey(tag))
            {
                return true; // Hits if the tag is at the row index
            }
            else { return false; }
        }

        /// <summary>
        /// Return 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int convertToBinary(int address){
            string binary;
            while (address > 0)
            {
                int temp = address % 2; // If its odd then temp will be 1
                binary += temp + "";
                address = address / 2;
            }
            int result; 
            Int32.TryParse(binary, out result);
            return result;
        }

    }
}
