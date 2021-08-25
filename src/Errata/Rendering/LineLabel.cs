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

        public LineLabel(LabelInfo label, TextSpan columns, int anchor)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            Columns = columns;
            Anchor = anchor;
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
