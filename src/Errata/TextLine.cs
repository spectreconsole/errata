using System;
using System.Collections.Generic;

namespace Errata
{
    public sealed class TextLine
    {
        public int Index { get; set; }
        public string Text { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }

        public Range Range => Offset..(Offset + Length);

        public TextLine(int index, string text, int offset)
        {
            Index = index;
            Text = text ?? string.Empty;
            Offset = offset;
            Length = Text.Length;
        }

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
