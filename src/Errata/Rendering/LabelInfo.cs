using System;
using Spectre.Console;

namespace Errata
{
    internal enum LabelKind
    {
        SingleLine,
        MultiLine,
    }

    internal sealed class LabelInfo
    {
        public string SourceId { get; }
        public TextSpan Span { get; }
        public string Message { get; }
        public Color Color { get; }
        public string? Note { get; }
        public LabelKind Kind { get; }

        public LabelInfo(
            string sourceId, TextSpan span, string message,
            Color color, string? note, LabelKind kind)
        {
            SourceId = sourceId ?? throw new ArgumentNullException(nameof(sourceId));
            Span = span;
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Color = color;
            Note = note;
            Kind = kind;
        }
    }
}
