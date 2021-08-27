using System;

namespace Errata
{
    internal sealed class LineLabel
    {
        /// <summary>
        /// Gets the label information.
        /// </summary>
        public LabelInfo Label { get; }

        /// <summary>
        /// Gets the column span for the label in the line.
        /// </summary>
        public TextSpan Columns { get; }

        /// <summary>
        /// Gets the anchor column index.
        /// The anchor is the position a label attaches to a line.
        /// </summary>
        public int Anchor { get; }

        public int Priority => Label.Priority;

        /// <summary>
        /// Gets a value indicating whether or not the message should be rendered for this line.
        /// </summary>
        public bool ShouldRenderMessage { get; }

        public bool IsMultiLine => Label.Kind == LabelKind.MultiLine;

        public LineLabel(LabelInfo label, TextSpan columns, int anchor, bool renderMessage)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
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
                    builder.Append(Character.Anchor, Label.Color);
                }
                else
                {
                    // 🔎 ─
                    builder.Append(Character.AnchorHorizontalLine, Label.Color);
                }
            }
        }
    }
}
