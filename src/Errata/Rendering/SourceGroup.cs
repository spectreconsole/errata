namespace Errata;

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

    /// <summary>
    /// Gets the set of line indices that are context-only lines (no labels).
    /// </summary>
    private HashSet<int> ContextLineIndices { get; }

    public SourceGroup(Source source, IEnumerable<LabelInfo> labels)
    {
        Source = source;
        Labels = new List<LabelInfo>(labels);
        ContextLineIndices = new HashSet<int>();

        var min = Labels.Min(info => info.SourceSpan.Start);
        var max = Labels.Max(label => label.SourceSpan.End);

        // Calculate context lines for each label
        foreach (var label in Labels)
        {
            if (label.ContextLines <= 0)
            {
                continue;
            }

            // Find the line index where this label starts
            var labelLineIndex = source.GetLineOffset(label.SourceSpan.Start).LineIndex;

            // Add context lines above this label
            for (var i = 1; i <= label.ContextLines; i++)
            {
                var contextLineIndex = labelLineIndex - i;
                if (contextLineIndex >= 0)
                {
                    ContextLineIndices.Add(contextLineIndex);

                    // Expand the span to include this context line
                    var contextLine = source.Lines[contextLineIndex];
                    min = Math.Min(min, contextLine.Offset);
                }
            }
        }

        Span = new TextSpan(min, max);
    }

    public IReadOnlyList<LineLabel> GetLabelsForLine(TextLine line)
    {
        var result = new List<LineLabel>();

        var labels = Labels.Where(label => label.SourceSpan.Start >= line.Span.Start && label.SourceSpan.End <= line.Span.End);
        foreach (var label in labels)
        {
            var anchor = ((label.SourceSpan.Start + label.SourceSpan.End) / 2) - line.Offset;
            var columns = new TextSpan(
                label.SourceSpan.Start - line.Offset,
                Math.Min(label.SourceSpan.End - line.Offset, line.Length));

            result.Add(new LineLabel(label, columns, anchor, renderMessage: true));
        }

        return new List<LineLabel>(
            result
                .Where(l => !l.IsMultiLine)
                .OrderBy(l => l.Priority)
                .ThenBy(l => l.Columns.Start));
    }

    public bool IsContextLine(int lineIndex)
    {
        return ContextLineIndices.Contains(lineIndex);
    }
}
