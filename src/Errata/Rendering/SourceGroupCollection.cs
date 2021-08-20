using System;
using System.Collections.Generic;
using System.Linq;
using Errata.Rendering;

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

                var info = new LabelInfo(label.SourceId, label.GetSpan(source), label.Message, label.Color, label.Note);

                var startLine = source.GetLineOffset(info.Span.Start).LineIndex;
                var endLine = source.GetLineOffset(info.Span.End).LineIndex;

                if (!groups.TryGetValue(source, out var g))
                {
                    groups.Add(source, new List<LabelInfo>());
                }

                groups[source].Add(info);
            }

            return new SourceGroupCollection(
                groups.Select(group => new SourceGroup(group.Key, group.Value)));
        }

        public int GetLineNumberMaxWidth()
        {
            return this.Max(group =>
            {
                var lineRange = group.Source.GetLineSpan(group.Span);
                var end = lineRange.End.Value == 0 ? 1 : lineRange.End.Value;
                return (int)(Math.Log10(end) + 1);
            });
        }
    }
}
