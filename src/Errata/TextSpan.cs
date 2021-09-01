using System;
using System.Collections;
using System.Collections.Generic;

namespace Errata
{
    /// <summary>
    /// Represents a range that has start and end indexes.
    /// </summary>
    public readonly struct TextSpan : IEnumerable<int>, IEquatable<TextSpan>
    {
        /// <summary>
        /// Gets the start position.
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// Gets the end position.
        /// </summary>
        public int End { get; }

        /// <summary>
        /// Gets the length.
        /// </summary>
        public int Length => End - Start;

        /// <summary>
        /// Gets the last offset in the span.
        /// </summary>
        public int LastOffset => Math.Max(Start, End - 1);

        private sealed class Enumerator : IEnumerator<int>
        {
            private readonly TextSpan _span;

            public int Current { get; private set; }
            object IEnumerator.Current => Current;

            public Enumerator(TextSpan span)
            {
                _span = span;
                Current = _span.Start - 1;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (Current >= _span.End - 1)
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

        /// <summary>
        /// Initializes a new instance of the <see cref="TextSpan"/> struct.
        /// </summary>
        /// <param name="start">The start position.</param>
        /// <param name="end">The end position.</param>
        public TextSpan(int start, int end)
        {
            if (end < start)
            {
                throw new ArgumentException("The end position must be greater than the start position", nameof(end));
            }

            Start = start;
            End = end;
        }

#if NET5_0_OR_GREATER
        /// <summary>
        /// Initializes a new instance of the <see cref="TextSpan"/> struct.
        /// </summary>
        /// <param name="range">The range.</param>
        public TextSpan(Range range)
        {
            if (range.Start.IsFromEnd)
            {
                throw new InvalidOperationException("Start index cannot begin from end");
            }

            if (range.End.IsFromEnd)
            {
                throw new InvalidOperationException("Start end cannot begin from end");
            }

            if (range.End.Value < range.Start.Value)
            {
                throw new ArgumentException("The end position must be greater than the start position", nameof(range));
            }

            Start = range.Start.Value;
            End = range.End.Value;
        }
#endif

        /// <summary>
        /// Determines whether or not the specified offset
        /// exist within the span.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns><c>true</c> if the specified offset exist within the span, otherwise <c>false</c>.</returns>
        public bool Contains(int offset)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be equal or greater than zero (0)");
            }

            return Start <= offset && LastOffset >= offset;
        }

        /// <inheritdoc/>
        public bool Equals(TextSpan other)
        {
            return Start == other.Start && End == other.End;
        }

        /// <inheritdoc/>
        public IEnumerator<int> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is TextSpan span && Equals(span);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override string? ToString()
        {
            return $"{Start}..{End}";
        }

        /// <summary>
        /// Checks if two <see cref="TextSpan"/> instances are equal.
        /// </summary>
        /// <param name="left">The first text span instance to compare.</param>
        /// <param name="right">The second text span instance to compare.</param>
        /// <returns><c>true</c> if the two text spans are equal, otherwise <c>false</c>.</returns>
        public static bool operator ==(TextSpan left, TextSpan right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks if two <see cref="TextSpan"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first text span instance to compare.</param>
        /// <param name="right">The second text span instance to compare.</param>
        /// <returns><c>true</c> if the two text spans are not equal, otherwise <c>false</c>.</returns>
        public static bool operator !=(TextSpan left, TextSpan right)
        {
            return !(left == right);
        }
    }
}
