#if NET5_0_OR_GREATER
using System;

namespace Errata
{
    internal static class RangeExtensions
    {
        public static bool Contains(this Range range, int value)
        {
            return range.Start.Value >= value && range.End.Value <= value;
        }
    }
}
#endif
