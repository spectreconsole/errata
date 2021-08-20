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

        public (TextLine Line, int LineIndex, int ColumnIndex) GetLineOffset(int offset)
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

        public Range GetLineSpan(Range span)
        {
            var start = GetLineOffset(span.Start.Value).LineIndex;
            var end = GetLineOffset(Math.Max(span.Start.Value, span.End.Value)).LineIndex;
            return start..end;
        }

        public Range GetSpan(Location location, int length)
        {
            var row = location.Row - 1;
            var column = location.Column - 1;

            if (row >= Lines.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Label row exceeded number of rows");
            }

            var line = Lines[row];
            if (column >= line.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "label column cannot start at the end of the line");
            }

            // Trying to grab text outide of the line?
            if (column + length > line.Length)
            {
                // Adjust the length
                length = line.Length - column;
            }

            return (line.Offset + column)..(line.Offset + column + length);
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
