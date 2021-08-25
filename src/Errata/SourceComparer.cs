using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Errata
{
    internal sealed class SourceComparer : IEqualityComparer<Source>
    {
        public static SourceComparer Shared { get; } = new SourceComparer();

        public bool Equals(Source? x, Source? y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }

            return x.Id.Equals(y.Id, StringComparison.Ordinal);
        }

        public int GetHashCode([DisallowNull] Source obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
