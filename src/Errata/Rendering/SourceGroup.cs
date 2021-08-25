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

            var min = Labels.Min(info => info.SourceSpan.Start);
            var max = Labels.Max(label => label.SourceSpan.End);
            Span = new TextSpan(min, max);
        }

        public IReadOnlyList<LineLabel> GetLabelsForLine(TextLine line)
        {
            var lineLables = new List<LineLabel>();
            var labels = Labels.Where(label => label.SourceSpan.Start >= line.Span.Start && label.SourceSpan.End <= line.Span.End);
            foreach (var label in labels)
            {
                var anchor = ((label.SourceSpan.Start + label.SourceSpan.End) / 2) - line.Offset;
                var span = new TextSpan(
                    label.SourceSpan.Start - line.Offset,
                    label.SourceSpan.End - line.Offset);

                lineLables.Add(new LineLabel(label, span, anchor));
            }

            return new List<LineLabel>(lineLables.OrderBy(l => l.Columns.Start));
        }
    }
}
