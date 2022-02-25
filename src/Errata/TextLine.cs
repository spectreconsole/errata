using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Errata
{
    /// <summary>
    /// Represents a text line.
    /// </summary>
    [DebuggerDisplay("{Text}")]
    public sealed class TextLine
    {
        /// <summary>
        /// Gets the line index.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets the line offset.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// Gets the line break.
        /// </summary>
        public char[] LineBreak { get; }

        /// <summary>
        /// Gets the line text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the line length.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets the character span for the line.
        /// </summary>
        public TextSpan Span { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextLine"/> class.
        /// </summary>
        /// <param name="index">The line index.</param>
        /// <param name="text">The line text.</param>
        /// <param name="offset">The line offset.</param>
        /// <param name="lineBreak">The line break.</param>
        public TextLine(int index, string text, int offset, char[]? lineBreak = null)
        {
            Index = index;
            Text = text ?? string.Empty;
            Offset = offset;
            LineBreak = lineBreak ?? Array.Empty<char>();
            Length = Text.Length;
            Span = new TextSpan(Offset, Offset + Length);
        }

        /// <summary>
        /// Splits the provided text into a list of <see cref="TextLine"/>.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <returns>A list of <see cref="TextLine"/>.</returns>
        public static List<TextLine> Split(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var lines = new List<TextLine>();
            var buffer = new TextBuffer(text);
            var index = 0;
            var offset = 0;

            while (buffer.CanRead)
            {
                var current = buffer.Read();
                if (current == '\r' && buffer.Peek(0) == '\n')
                {
                    buffer.Read(); // Consume the \n

                    var line = new TextLine(
                        index, buffer.Slice(offset, buffer.Position - 2),
                        offset, new char[] { '\r', '\n' });

                    lines.Add(line);
                    offset += line.Length + line.LineBreak.Length;
                    index++;
                }
                else if (current == '\n')
                {
                    var line = new TextLine(
                        index, buffer.Slice(offset, buffer.Position - 1),
                        offset, new char[] { '\n' });

                    lines.Add(line);
                    offset += line.Length + line.LineBreak.Length;
                    index++;
                }
            }

            if (offset < buffer.Length)
            {
                lines.Add(
                    new TextLine(
                        index,
                        buffer.Slice(offset, buffer.Position),
                        offset,
                        null));
            }

            return lines;
        }
    }
}
