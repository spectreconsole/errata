using System;
using System.Collections.Generic;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Errata
{
    internal sealed class ReportRendererContext
    {
        private readonly List<Segment> _buffer;
        private readonly List<SegmentLine> _lines;

        public CharacterSet CharacterSet { get; }
        public ReportSettings Settings { get; }

        public ReportRendererContext(IAnsiConsole console, ReportSettings? settings)
        {
            _buffer = new List<Segment>();
            _lines = new List<SegmentLine>();

            Settings = settings ?? new ReportSettings();
            CharacterSet = Settings.Characters ??= CharacterSet.Create(console);
        }

        public void Append(Character character, Color? color = null, Decoration? decoration = null)
        {
            Append(CharacterSet.Get(character), color, decoration);
        }

        public void Append(string text, Color? color = null, Decoration? decoration = null)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            _buffer.Add(new Segment(text, new Style(foreground: color, decoration: decoration)));
        }

        public void Append(char character, Color? color = null, Decoration? decoration = null)
        {
            _buffer.Add(new Segment(new string(character, 1), new Style(foreground: color, decoration: decoration)));
        }

        public void AppendRepeated(Character character, int count, Color? color = null)
        {
            AppendRepeated(CharacterSet.Get(character), count, color);
        }

        public void AppendRepeated(char character, int count, Color? color = null)
        {
            Append(new string(character, count), color);
        }

        public void AppendSpace()
        {
            Append(" ");
        }

        public void AppendSpaces(int count)
        {
            if (count > 0)
            {
                Append(new string(' ', count));
            }
        }

        public void CommitLine()
        {
            if (_buffer.Count == 0)
            {
                AppendSpace();
            }

            _lines.Add(new SegmentLine(_buffer));
            _buffer.Clear();
        }

        public IReadOnlyList<SegmentLine> GetLines()
        {
            return _lines;
        }
    }
}
