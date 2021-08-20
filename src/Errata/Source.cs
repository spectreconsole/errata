using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Errata
{
    public sealed class Source
    {
        public string Id { get; set; }
        public string Name { get; }
        public List<TextLine> Lines { get; set; }
        public int Length { get; set; }
        public string Text { get; set; }

        public static IEqualityComparer<Source> Comparer => SourceComparer.Shared;

        public Source(string id, string text)
            : this(id, id, text)
        {
        }

        public Source(string id, string name, string text)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Lines = TextLine.Split(text);
            Length = Lines.Sum(x => x.Length) + Lines.Count - 1;
            Text = text ?? string.Empty;
        }

        private sealed class SourceComparer : IEqualityComparer<Source>
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

        public (TextLine Line, int LineIndex, int ColumnIndex) GetOffsetLine(int offset)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be equal or greater than zero (0)");
            }

            if (offset >= Length)
            {
                throw new InvalidOperationException("Invalid offset");
            }

            var lineIndex = GetLineIndex(offset);
            var line = Lines[lineIndex];
            var columnIndex = offset - line.Offset;

            return (line, lineIndex, columnIndex);
        }

        public Range GetLineRange(Range span)
        {
            var start = GetOffsetLine(span.Start.Value).LineIndex;
            var end = GetOffsetLine(Math.Max(span.Start.Value, span.End.Value)).LineIndex;
            return start..end;
        }

        private int GetLineIndex(int offset)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be equal or greater than zero (0)");
            }

            var index = 0;
            foreach (var line in Lines)
            {
                if (offset >= line.Offset && offset <= line.Offset + line.Length)
                {
                    return index;
                }

                index++;
            }

            throw new InvalidOperationException("Line index could not be found");
        }
    }
}
