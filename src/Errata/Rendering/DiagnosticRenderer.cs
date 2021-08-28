using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

namespace Errata
{
    internal sealed class DiagnosticRenderer
    {
        private readonly ReportContext _reportContext;

        public DiagnosticRenderer(ReportContext context)
        {
            _reportContext = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        public void Render(Diagnostic diagnostic)
        {
            var ctx = _reportContext.CreateDiagnosticContext(diagnostic);

            // 🔎 Error [ABC123]: This is the error\n
            var prefix = ctx.Formatter.Format(diagnostic);
            if (prefix != null)
            {
                ctx.Builder.AppendMarkup(prefix);
                ctx.Builder.CommitLine();
            }

            if (!string.IsNullOrWhiteSpace(diagnostic.Note))
            {
                // 🔎 NOTE: This is a note\n
                ctx.Builder.Append("NOTE: ", Color.Aqua);
                ctx.Builder.Append(diagnostic.Note);
                ctx.Builder.CommitLine();
            }

            // Iterate all source groups
            foreach (var (_, first, last, group) in ctx.Groups.Enumerate())
            {
                // 🔎 ···┌─[Program.cs]\n
                ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                ctx.Builder.Append(first ? Character.TopLeftCornerHard : Character.LeftConnector, Color.Grey);
                ctx.Builder.Append(Character.HorizontalLine, Color.Grey);
                ctx.Builder.Append("[", Color.Grey);
                ctx.Builder.Append(group.Source.Name, Color.White);
                ctx.Builder.Append("]", Color.Grey);
                ctx.Builder.CommitLine();

                // 🔎 ···│\n
                ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                ctx.Builder.Append(Character.VerticalLine, Color.Grey);
                ctx.Builder.CommitLine();

                // Get all multi line labels
                var multiLabels = group.Labels
                    .Where(l => l.Kind == LabelKind.MultiLine)
                    .OrderBy(l => l.SourceSpan.Length)
                    .ToList();

                // Iterate all lines in the line range
                foreach (var (_, _, lastLine, lineIndex) in group.Source.GetLineRange(group.Span).Enumerate())
                {
                    // Get the current line
                    var line = group.Source.Lines[lineIndex];

                    // Get all labels for the current line
                    var labels = group.GetLabelsForLine(line);
                    if (labels.Count == 0)
                    {
                        continue;
                    }

                    // Write text line
                    RenderText(ctx, line, labels);

                    // Write labels
                    foreach (var (row, label) in labels.EnumerateWithIndex())
                    {
                        // Render anchors and vertical lines
                        RenderVerticalLines(ctx, line, labels, row);

                        // Render the horizontal lines
                        RenderHorizontalLines(ctx, line, labels, row, label);

                        // Render the label message
                        if (label.ShouldRenderMessage)
                        {
                            // 🔎 The label message
                            ctx.Builder.AppendSpace();
                            ctx.Builder.Append(label.Message, label.Color);
                        }

                        // 🔎 \n
                        ctx.Builder.CommitLine();
                    }

                    if (!lastLine)
                    {
                        // 🔎 ···(dot)\n
                        ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                        ctx.Builder.Append(Character.Dot, Color.Grey);
                        ctx.Builder.CommitLine();
                    }
                }

                // 🔎 ···(separator)\n
                var separator = last ? Character.VerticalLine : Character.Dot;
                ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                ctx.Builder.Append(separator, Color.Grey);
                ctx.Builder.CommitLine();

                // Got labels with notes?
                var labelsWithNotes = group.Labels.Where(l => l.Note != null).ToArray();
                if (labelsWithNotes != null)
                {
                    foreach (var (_, firstLabel, lastLabel, labelWithNote) in labelsWithNotes.Enumerate())
                    {
                        // Got a note?
                        if (!string.IsNullOrWhiteSpace(labelWithNote.Note))
                        {
                            // 🔎 ···(dot) NOTE: This is a note\n
                            ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                            ctx.Builder.Append(Character.Dot, Color.Grey);
                            ctx.Builder.AppendSpace();
                            ctx.Builder.Append("NOTE: ", Color.Aqua);
                            ctx.Builder.Append(labelWithNote.Note);
                            ctx.Builder.CommitLine();
                        }

                        if (lastLabel)
                        {
                            // 🔎 ···│\n
                            ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                            ctx.Builder.Append(Character.VerticalLine, Color.Grey);
                            ctx.Builder.CommitLine();
                        }
                    }
                }

                if (last)
                {
                    // 🔎 ···└─\n
                    ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                    ctx.Builder.Append(Character.BottomLeftCornerHard, Color.Grey);
                    ctx.Builder.Append(Character.HorizontalLine, Color.Grey);
                    ctx.Builder.CommitLine();
                }
            }
        }

        private static void RenderText(DiagnosticContext ctx, TextLine line, IReadOnlyList<LineLabel> labels)
        {
            RenderMargin(ctx, line, true);
            ctx.Builder.AppendSpace();
            foreach (var (column, character) in line.Text.EnumerateWithIndex())
            {
                var color = GetHighlightColor(labels, line, column);
                ctx.Builder.Append(character, color);
            }

            ctx.Builder.CommitLine();
        }

        private static void RenderVerticalLines(
            DiagnosticContext ctx, TextLine line,
            IReadOnlyList<LineLabel> labels, int row)
        {
            var result = new (char, Color?)[line.Text.Length];
            var crucial = false;

            for (var col = 0; col < line.Text.Length; col++)
            {
                var anchor = GetAnchor(labels, col, row);
                var underline = GetHighlightColor(labels, line, col);

                if (row != 0)
                {
                    underline = null;
                }

                if (anchor != null)
                {
                    if (underline != null)
                    {
                        // 🔎 ┬
                        result[col] = (ctx.Characters.Get(Character.Anchor), underline);
                        crucial = true;
                    }
                    else
                    {
                        // 🔎 │
                        result[col] = (ctx.Characters.Get(Character.AnchorVerticalLine), anchor.Color);
                    }
                }
                else if (underline != null)
                {
                    // 🔎 ─
                    result[col] = (ctx.Characters.Get(Character.AnchorHorizontalLine), underline);
                    crucial = true;
                }
                else
                {
                    // 🔎 ·
                    result[col] = (' ', null);
                }
            }

            // Rendering compact reports and
            // nothing crucial was detected for the line?
            if (ctx.Compact && !crucial)
            {
                return;
            }

            RenderMargin(ctx, line, false);
            ctx.Builder.AppendSpace();
            foreach (var (character, color) in result)
            {
                ctx.Builder.Append(character, color);
            }

            ctx.Builder.CommitLine();
        }

        private static void RenderHorizontalLines(
            DiagnosticContext ctx, TextLine line,
            IReadOnlyList<LineLabel> labels,
            int row, LineLabel label)
        {
            RenderMargin(ctx, line, false);
            ctx.Builder.AppendSpace();

            for (var col = 0; col < line.Text.Length; col++)
            {
                var anchor = GetAnchor(labels, col, row);
                var hasHorizontalBar = !string.IsNullOrWhiteSpace(label.Message)
                    && label.ShouldRenderMessage
                    && col > label.Anchor;

                if (col == label.Anchor && !string.IsNullOrWhiteSpace(label.Message))
                {
                    // 🔎 ╰
                    ctx.Builder.Append(Character.BottomLeftCornerRound, label.Color);
                }
                else if (anchor != null && (col != label.Anchor || !string.IsNullOrWhiteSpace(label.Message)))
                {
                    if (hasHorizontalBar)
                    {
                        // 🔎 ─
                        ctx.Builder.Append(Character.HorizontalLine, label.Color);
                    }
                    else
                    {
                        // 🔎 │
                        ctx.Builder.Append(Character.VerticalLine, anchor.Color);
                    }
                }
                else if (hasHorizontalBar)
                {
                    // 🔎 ─
                    ctx.Builder.Append(Character.HorizontalLine, label.Color);
                }
                else
                {
                    // 🔎 ·
                    ctx.Builder.Append(' ', label.Color);
                }
            }
        }

        private static void RenderMargin(
            DiagnosticContext ctx,
            TextLine line,
            bool showLineNumber)
        {
            if (showLineNumber)
            {
                // 🔎 ·38·│
                ctx.Builder.AppendSpace();
                ctx.Builder.Append((line.Index + 1).ToString().PadRight(ctx.LineNumberWidth));
                ctx.Builder.AppendSpace();
                ctx.Builder.Append(Character.VerticalLine, Color.Grey);
            }
            else
            {
                // 🔎 ····(dot)
                ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                ctx.Builder.Append(Character.Dot, Color.Grey);
            }
        }

        private static LineLabel? GetAnchor(IEnumerable<LineLabel> labels, int column, int row)
        {
            foreach (var (index, label) in labels.EnumerateWithIndex())
            {
                if (string.IsNullOrWhiteSpace(label.Message))
                {
                    continue;
                }

                if (label.Anchor == column && row <= index)
                {
                    return label;
                }
            }

            return null;
        }

        private static Color? GetHighlightColor(IEnumerable<LineLabel> labels, TextLine line, int column)
        {
            return labels.Where(label => label.SourceSpan.Contains(line.Offset + column))
                .OrderBy(l => l.Priority)
                .ThenBy(l => l.Columns.Start)
                .FirstOrDefault()?.Color;
        }
    }
}