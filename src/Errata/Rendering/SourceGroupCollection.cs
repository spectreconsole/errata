using System;
using System.Collections.Generic;
using System.Linq;

namespace Errata
{
    internal sealed class SourceGroupCollection : List<SourceGroup>
    {
        public SourceGroupCollection(IEnumerable<SourceGroup> collection)
            : base(collection)
        {
        }

        public static SourceGroupCollection CreateFromLabels(ISourceRepository repository, List<Label> labels)
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (labels is null)
            {
                throw new ArgumentNullException(nameof(labels));
            }

            var groups = new Dictionary<Source, List<LabelInfo>>(Source.Comparer);

            foreach (var label in labels)
            {
                if (!repository.TryGet(label.SourceId, out var source))
                {
                    throw new InvalidOperationException($"Could not get source for '{label.SourceId}'");
                }

                if (!groups.TryGetValue(source, out var _))
                {
                    groups.Add(source, new List<LabelInfo>());
                }

                var span = label.GetSpan(source);
                var startLine = source.GetLineOffset(span.Start).LineIndex;
                var endLine = source.GetLineOffset(span.End).LineIndex;
                var kind = startLine == endLine ? LabelKind.SingleLine : LabelKind.MultiLine;

                groups[source].Add(
                    new LabelInfo(
                        label.SourceId, span, label.Message,
                        label.Color, label.Note, kind));
            }

            return new SourceGroupCollection(
                groups.Select(group => new SourceGroup(group.Key, group.Value)));
        }

        public int GetLineNumberMaxWidth()
        {
            if (Count == 0)
            {
                return 0;
            }

            return this.Max(group =>
            {
                var lineRange = group.Source.GetLineSpan(group.Span);
                var end = lineRange.End == 0 ? 1 : lineRange.End;
                return (int)(Math.Log10(end) + 1);
            });
        }
    }
}
