using System;
using System.Collections;
using System.Collections.Generic;

namespace Errata
{
    internal readonly struct LineRange : IEnumerable<int>, IEquatable<LineRange>
    {
        public int Start { get; }
        public int End { get; }
        public int Length => End - Start;

        public bool IsMultiLine => Start != End;

        private sealed class Enumerator : IEnumerator<int>
        {
            private readonly LineRange _span;

            public int Current { get; private set; }
            object IEnumerator.Current => Current;

            public Enumerator(LineRange span)
            {
                _span = span;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (Current >= _span.End)
                {
                    return false;
                }

                Current++;
                return true;
            }

            public void Reset()
            {
                Current = _span.Start - 1;
            }
        }

        public LineRange(int start, int end)
        {
            if (end < start)
            {
                throw new ArgumentException("The end position must be greater than the start position", nameof(end));
            }

            Start = start;
            End = end;
        }

        public bool Contains(int offset)
        {
            if (offset < 0)
            {
                throw new ErrataException("Offset must be equal or greater than zero (0)")
                    .WithContext("Start", this.Start)
                    .WithContext("End", this.End)
                    .WithContext("Multiline", this.IsMultiLine)
                    .WithContext("Length", Length)
                    .WithContext("Offset", offset);
            }

            return Start <= offset && End > offset;
        }

        public bool Equals(LineRange other)
        {
            return Start == other.Start && End == other.End;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Equals(object? obj)
        {
            return obj is LineRange span && Equals(span);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)2166136261;
                hash = (hash * 16777619) ^ Start.GetHashCode();
                hash = (hash * 16777619) ^ End.GetHashCode();
                return hash;
            }
        }

        public override string? ToString()
        {
            return $"{Start}..{End}";
        }

        public static bool operator ==(LineRange left, LineRange right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LineRange left, LineRange right)
        {
            return !(left == right);
        }
    }
}
