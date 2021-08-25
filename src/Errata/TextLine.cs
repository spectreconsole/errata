using System;
using System.Collections.Generic;

namespace Errata
{
    /// <summary>
    /// Represents a text line.
    /// </summary>
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
        public TextLine(int index, string text, int offset)
        {
            Index = index;
            Text = text ?? string.Empty;
            Offset = offset;
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

            var result = new List<TextLine>();
            var lines = text.Replace("\r\n", "\n").Split('\n'); // TODO: Handle line breaks properly
            var offset = 0;
            foreach (var (index, _, _, line) in lines.Enumerate())
            {
                var tl = new TextLine(index, line, offset);
                result.Add(tl);
                offset += tl.Length + 1;
            }

            return result;
        }
    }
}
