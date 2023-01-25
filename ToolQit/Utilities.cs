using System;

namespace ToolQit
{
    public static class Utilities
    {
        private static readonly string[] SizeSuffixes =
            { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        // Stolen from stackoverflow!
        // https://stackoverflow.com/a/14488941/9948300
        /// <summary>
        /// Convert bytes to the correct size suffix.
        /// </summary>
        /// <param name="bytes">The bytes that need to be converted to the corresponding suffix</param>
        /// <param name="decimalPlaces">Set the decimal places for the suffix</param>
        /// <returns>The suffix string</returns>
        /// <exception cref="ArgumentOutOfRangeException">Exception is thrown when the decimalPlaces arg is zero or negative</exception>
        public static string SizeSuffix(long bytes, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0)
                throw new ArgumentOutOfRangeException(nameof(decimalPlaces));
            switch (bytes)
            {
                case < 0:
                    return "-" + SizeSuffix(-bytes, decimalPlaces);
                case 0:
                    return string.Format("{0:n" + decimalPlaces + "} bytes", 0);
            }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(bytes, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)bytes / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            
            return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, SizeSuffixes[mag]);
        }
    }
}