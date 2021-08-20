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

            var groups = new Dictionary<Source, List<Label>>(Source.Comparer);

            foreach (var label in labels)
            {
                if (!repository.TryGet(label.SourceId, out var source))
                {
                    throw new InvalidOperationException($"Could not get source for '{label.SourceId}'");
                }

                var startLine = source.GetOffsetLine(label.Span.Start).LineIndex;
                var endLine = source.GetOffsetLine(label.Span.End).LineIndex;

                if (!groups.TryGetValue(source, out var g))
                {
                    groups.Add(source, new List<Label>());
                }

                groups[source].Add(label);
            }

            return new SourceGroupCollection(
                groups.Select(group => new SourceGroup(group.Key, group.Value)));
        }

        public int GetLineNumberMaxWidth()
        {
            return this.Max(group =>
            {
                var lineRange = group.Source.GetLineRange(group.Span);
                var end = lineRange.End.Value == 0 ? 1 : lineRange.End.Value;
                return (int)(Math.Log10(end) + 1);
            });
        }
    }
}
