using System;
using Spectre.Console;

namespace Errata
{
    internal sealed class LabelInfo
    {
        public string SourceId { get; }
        public TextSpan Span { get; }
        public string Message { get; }
        public Color Color { get; set; }
        public string? Note { get; set; }

        public LabelInfo(string sourceId, Range span, string message, Color color, string? note)
        {
            SourceId = sourceId ?? throw new ArgumentNullException(nameof(sourceId));
            Span = new TextSpan(sourceId, span);
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Color = color;
            Note = note;
        }
    }
}
