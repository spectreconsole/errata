using System;
using Spectre.Console;

namespace Errata
{
    internal sealed class LabelInfo
    {
        public string SourceId { get; }
        public TextSpan SourceSpan { get; }
        public Label Label { get; }
        public LabelKind Kind => Lines.IsMultiLine ? LabelKind.MultiLine : LabelKind.Inline;
        public LineRange Lines { get; }

        public Color? Color => Label.Color;
        public string Message => Label.Message;
        public string? Note => Label.Note;

        public LabelInfo(
            string sourceId, TextSpan sourceSpan, Label label,
            LineRange lines)
        {
            SourceId = sourceId ?? throw new ArgumentNullException(nameof(sourceId));
            SourceSpan = sourceSpan;
            Label = label ?? throw new ArgumentNullException(nameof(label));
            Lines = lines;
        }
    }
}
