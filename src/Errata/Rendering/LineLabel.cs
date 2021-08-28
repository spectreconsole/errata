using System;
using Spectre.Console;

namespace Errata
{
    internal sealed class LineLabel
    {
        private readonly LabelInfo _label;

        /// <summary>
        /// Gets the column span for the label in the line.
        /// </summary>
        public TextSpan Columns { get; }

        /// <summary>
        /// Gets the anchor column index.
        /// The anchor is the position a label attaches to a line.
        /// </summary>
        public int Anchor { get; }

        /// <summary>
        /// Gets a value indicating whether or not the message should be rendered for this line.
        /// </summary>
        public bool ShouldRenderMessage { get; }

        public int Priority => _label.Priority;
        public bool IsMultiLine => _label.Kind == LabelKind.MultiLine;
        public TextSpan SourceSpan => _label.SourceSpan;
        public string Message => _label.Message;
        public Color? Color => _label.Color;

        public LineLabel(LabelInfo label, TextSpan columns, int anchor, bool renderMessage)
        {
            _label = label ?? throw new ArgumentNullException(nameof(label));

            Columns = columns;
            Anchor = anchor;
            ShouldRenderMessage = renderMessage;
        }

        public void DrawAnchor(ReportBuilder builder)
        {
            foreach (var column in Columns)
            {
                if (column == Anchor)
                {
                    // 🔎 ┬
                    builder.Append(Character.Anchor, _label.Color);
                }
                else
                {
                    // 🔎 ─
                    builder.Append(Character.AnchorHorizontalLine, _label.Color);
                }
            }
        }
    }
}
