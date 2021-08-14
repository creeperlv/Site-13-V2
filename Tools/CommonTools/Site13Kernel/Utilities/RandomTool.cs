using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Utilities
{
    public class RandomTool
    {
        static Random random;
        public static void Init()
        {
            random = new Random();
        }
        public static void Init(int Seed)
        {
            random = new Random(Seed);
        }
        public static int NextInt()
        {
            return random.Next();
        }
        public static int NextInt(int Upper)
        {
            return random.Next(0, Upper);
        }
        public static int NextInt(int Lower, int Upper) => random.Next(Lower, Upper);
    }
}
