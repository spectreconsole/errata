using System;
using Spectre.Console;

namespace Errata
{
    public sealed class Label
    {
        public string SourceId { get; }
        public TextSpan Span { get; }
        public string Message { get; }
        public Color Color { get; set; }
        public string? Note { get; set; }

        public Label(string sourceId, Range span, string message)
        {
            SourceId = sourceId ?? throw new ArgumentNullException(nameof(sourceId));
            Span = new TextSpan(sourceId, span);
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Color = Color.White;
        }

        public Label WithColor(Color color)
        {
            Color = color;
            return this;
        }

        public Label WithNote(string note)
        {
            if (note is null)
            {
                throw new ArgumentNullException(nameof(note));
            }

            Note = note;
            return this;
        }
    }
}
