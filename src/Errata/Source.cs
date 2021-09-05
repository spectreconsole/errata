using System;
using System.Collections.Generic;
using System.Linq;

namespace Errata
{
    /// <summary>
    /// Represents source code.
    /// </summary>
    public sealed class Source
    {
        /// <summary>
        /// Gets the source ID.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the name of the source.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the lines that the source is made up of.
        /// </summary>
        public List<TextLine> Lines { get; }

        /// <summary>
        /// Gets the character length of the source including line breaks.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets the source text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets a comparer that can be used to compare instances of <see cref="Source"/>.
        /// </summary>
        public static IEqualityComparer<Source> Comparer => SourceComparer.Shared;

        /// <summary>
        /// Initializes a new instance of the <see cref="Source"/> class.
        /// </summary>
        /// <param name="id">The source code ID.</param>
        /// <param name="text">The source code text.</param>
        public Source(string id, string text)
            : this(id, id, text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Source"/> class.
        /// </summary>
        /// <param name="id">The source code ID.</param>
        /// <param name="name">The name of the source.</param>
        /// <param name="text">The source code text.</param>
        public Source(string id, string name, string text)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Lines = TextLine.Split(text);
            Length = Lines.Sum(x => x.Length) + Lines.Count - 1;
            Text = text ?? string.Empty;
        }

        internal (TextLine Line, int LineIndex, int ColumnIndex) GetLineOffset(int offset)
        {
            if (offset < 0)
            {
                throw new ErrataException("Offset must be equal or greater than zero (0)")
                    .WithContext("Offset", offset);
            }

            if (offset > Length)
            {
                throw new ErrataException("Offset exceeded the source length")
                    .WithContext("Offset", offset)
                    .WithContext("Source length", Length);
            }

            var lineIndex = GetLineIndex(offset);
            var line = Lines[lineIndex];
            var columnIndex = offset - line.Offset;

            return (line, lineIndex, columnIndex);
        }

        internal LineRange GetLineRange(TextSpan span)
        {
            var start = GetLineOffset(span.Start).LineIndex;
            var end = GetLineOffset(Math.Max(span.Start, span.End)).LineIndex;
            return new LineRange(start, end);
        }

        internal TextSpan GetSourceSpan(Location location, int length)
        {
            var row = location.Row - 1;
            var column = location.Column - 1;

            if (row >= Lines.Count)
            {
                throw new ErrataException("Label row exceeded number of lines")
                    .WithContext("Row", row)
                    .WithContext("Column", column)
                    .WithContext("Line count", Lines.Count);
            }

            var line = Lines[row];
            if (column > line.Length - 1)
            {
                throw new ErrataException("label column cannot start at the end of the line")
                    .WithContext("Row", row)
                    .WithContext("Column", column)
                    .WithContext("Current line length", line.Length)
                    .WithContext("Line count", Lines.Count);
            }

            return new TextSpan(line.Offset + column, line.Offset + column + length);
        }

        private int GetLineIndex(int offset)
        {
            if (offset < 0)
            {
                throw new ErrataException("Offset must be equal or greater than zero (0)")
                    .WithContext("Offset", offset);
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

            throw new ErrataException("Line index could not be found")
                .WithContext("Offset", offset)
                .WithContext("Line count", Lines.Count);
        }
    }
}
