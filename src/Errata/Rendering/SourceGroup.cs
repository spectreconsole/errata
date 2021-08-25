using System.Collections.Generic;
using System.Linq;

namespace Errata
{
    internal sealed class SourceGroup
    {
        public Source Source { get; }
        public TextSpan Span { get; }
        public IReadOnlyList<LabelInfo> Labels { get; }

        public SourceGroup(Source source, IEnumerable<LabelInfo> labels)
        {
            Source = source;
            Labels = new List<LabelInfo>(labels);

            var min = Labels.Min(info => info.Span.Start);
            var max = Labels.Max(label => label.Span.End);
            Span = new TextSpan(min, max);
        }

        public IReadOnlyList<LineLabel> GetLabelsForLine(TextLine line)
        {
            var lineLables = new List<LineLabel>();
            var labels = Labels.Where(label => label.Span.Start >= line.Span.Start && label.Span.End <= line.Span.End);
            foreach (var label in labels)
            {
                var anchor = ((label.Span.Start + label.Span.End) / 2) - line.Offset;
                var span = new TextSpan(
                    label.Span.Start - line.Offset,
                    label.Span.End - line.Offset);

                lineLables.Add(new LineLabel(label, span, anchor));
            }

            return new List<LineLabel>(lineLables.OrderBy(l => l.Span.Start));
        }
    }
}
