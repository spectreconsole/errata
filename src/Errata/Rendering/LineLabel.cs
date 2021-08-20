using System;

namespace Errata
{
    internal sealed class LineLabel
    {
        public Label Label { get; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Anchor { get; set; }

        public LineLabel(Label label)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
        }
    }
}
