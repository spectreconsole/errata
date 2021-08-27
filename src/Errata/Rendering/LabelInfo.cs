using System;
using Spectre.Console;

namespace Errata
{
    internal sealed class LabelInfo
    {
        /// <summary>
        /// Gets the source ID.
        /// </summary>
        public string SourceId { get; }

        /// <summary>
        /// Gets the labels span within the source.
        /// </summary>
        public TextSpan SourceSpan { get; }

        /// <summary>
        /// Gets the label.
        /// </summary>
        public Label Label { get; }

        /// <summary>
        /// Gets the label kind.
        /// </summary>
        public LabelKind Kind => Lines.IsMultiLine ? LabelKind.MultiLine : LabelKind.Inline;

        /// <summary>
        /// Gets the line range where the label appears.
        /// </summary>
        public LineRange Lines { get; }

        public Color? Color => Label.Color;
        public string Message => Label.Message;
        public string? Note => Label.Note;
        public int Priority => Label.Priority;

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
