using System;
using System.Collections.Generic;
using System.Linq;

namespace TicoCinema.WebApplication.Utils
{
    public static class BitManager
    {
        public static long GetNextBitToAssign(long bitAssigned)
        {
            long nextBit = bitAssigned + bitAssigned;
            return nextBit;
        }

        public static bool ContainsBit(long categoriesSelected, long bitAssigned)
        {
            return (categoriesSelected & bitAssigned) > 0;
        }

        public static int GetBitsSum(List<long> categoriesSelected)
        {
            return (int)categoriesSelected.Sum();
        }
    }
}
