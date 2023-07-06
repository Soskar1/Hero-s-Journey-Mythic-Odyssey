#nullable enable
using System;

namespace HerosJourney.Utils
{
    public static class ThreadSafeRandom
    {
        [ThreadStatic]
        private static Random? _local;
        private static readonly Random Global = new();
        private static Random Instance
        {
            get
            {
                if (_local is null)
                {
                    int seed;
                    lock (Global)
                    {
                        seed = Global.Next();
                    }

                    _local = new Random(seed);
                }

                return _local;
            }
        }

        public static void SetSeed(int seed) => _local = new Random(seed);
        public static int Next() => Instance.Next();
        public static double NextDouble() => Instance.NextDouble();
    }
}