namespace TicoCinema.Utils
{
    public static class BitManager
    {
        public static long GetNextBitToAssign(long bitAssigned)
        {
            long nextBit = bitAssigned + bitAssigned;
            return nextBit;
        }
    }
}
