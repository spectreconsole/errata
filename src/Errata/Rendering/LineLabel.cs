using System;

namespace Errata
{
    internal sealed class LineLabel
    {
        public LabelInfo Label { get; }
        public TextSpan Span { get; }
        public int Anchor { get; set; }

        public LineLabel(LabelInfo label, TextSpan span, int anchor)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            Span = span;
            Anchor = anchor;
        }

        public void DrawAnchor(ReportBuilder builder)
        {
            for (var index = Span.Start; index < Span.End; index++)
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
