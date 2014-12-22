using System;

namespace Medici
{
    public static class ByteSize
    {
        public enum Scale
        {
            Bytes = 0,
            Kilobytes = 1,
            Megabytes = 2,
            Gigabytes = 3,
            Terabytes = 4
        };

        public const int Kilobyte = 1024;
        public const int Megabyte = 1024 * 1024;
        public const int Gigabyte = 1024 * 1024 * 1024;
        public const long Terabyte = 1024L * 1024L * 1024L * 1024L;

        public static long MakeSize(int amount, Scale scale)
        {
            switch(scale)
            {
                case Scale.Bytes:
                    return (long)amount;
                case Scale.Kilobytes:
                    return (long)amount * Kilobyte;
                case Scale.Megabytes:
                    return (long)amount * Megabyte;
                case Scale.Gigabytes:
                    return (long)amount * Gigabyte;
                case Scale.Terabytes:
                    return (long)amount * Terabyte;

                default:
                    throw new InvalidOperationException("Unknown scale.");
            }
        }
    }
}
