using System;

namespace Errata
{
    internal sealed class LineLabel
    {
        public LabelInfo Label { get; }
        public TextSpan Columns { get; }
        public int Anchor { get; set; }

        public LineLabel(LabelInfo label, TextSpan columns, int anchor)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            Columns = columns;
            Anchor = anchor;
        }

        public void DrawAnchor(ReportBuilder builder)
        {
            for (var index = Columns.Start; index < Columns.End; index++)
            {
                if (index == Anchor)
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
