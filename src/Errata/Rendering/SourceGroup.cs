using System;
using System.Collections.Generic;
using System.Linq;

namespace Errata
{
    internal sealed class SourceGroup
    {
        public Source Source { get; }
        public Range Span { get; }
        public IReadOnlyList<Label> Labels { get; }

        public SourceGroup(Source source, IEnumerable<Label> labels)
        {
            Source = source;
            Labels = new List<Label>(labels);

            var min = Labels.Min(info => info.Span.Start);
            var max = Labels.Max(label => label.Span.End);
            Span = min..max;
        }

        public IReadOnlyList<LineLabel> GetLabelsForLine(TextLine line)
        {
            var lineLables = new List<LineLabel>();
            var labels = Labels.Where(label => label.Span.Start >= line.Range.Start.Value && label.Span.End <= line.Range.End.Value);
            foreach (var label in labels)
            {
                lineLables.Add(new LineLabel(label)
                {
                    Start = label.Span.Start - line.Offset,
                    End = label.Span.End - line.Offset,
                    Anchor = ((label.Span.Start + label.Span.End) / 2) - line.Offset,
                });
            }

            return new List<LineLabel>(lineLables.OrderBy(l => l.Start));
        }
    }
}
