using System;

namespace Errata
{
    internal sealed class LineLabel
    {
        public LabelInfo Label { get; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Anchor { get; set; }

        public LineLabel(LabelInfo label)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
        }

        public void DrawAnchor(ReportBuilder builder)
        {
            for (var index = Start; index < End; index++)
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
