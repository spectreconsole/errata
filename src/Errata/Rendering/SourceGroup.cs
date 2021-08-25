using System;
using System.Collections.Generic;
using System.Linq;

namespace Errata
{
    internal sealed class SourceGroup
    {
        /// <summary>
        ///  Gets the source.
        /// </summary>
        public Source Source { get; }

        /// <summary>
        /// Gets the span within the source
        /// where labels appear.
        /// </summary>
        public TextSpan Span { get; }

        /// <summary>
        /// Gets all labels for this source.
        /// </summary>
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
                var columns = new TextSpan(
                    label.SourceSpan.Start - line.Offset,
                    Math.Min(label.SourceSpan.End - line.Offset, line.Length));

                lineLables.Add(new LineLabel(label, columns, anchor));
            }

            return new List<LineLabel>(lineLables.OrderBy(l => l.Columns.Start));
        }
    }
}
