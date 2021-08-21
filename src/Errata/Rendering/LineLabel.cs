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
    }
}
