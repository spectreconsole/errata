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

                    // Write text line with margin
                    RenderMargin(ctx, line, true);
                    ctx.Builder.AppendSpace();
                    foreach (var (column, character) in line.Text.EnumerateWithIndex())
                    {
                        var highlight = GetHighlightColor(labels, line, column);
                        ctx.Builder.Append(character, highlight);
                    }

                    ctx.Builder.CommitLine();

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
                            ctx.Builder.Append(label.Label.Message, label.Label.Color);
                        }

                        // 🔎 \n
                        ctx.Builder.CommitLine();
                    }

                    if (!lastLine)
                    {
                        // 🔎 ···│\n
                        ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                        ctx.Builder.Append(Character.VerticalLine, Color.Grey);
                        ctx.Builder.CommitLine();
                    }
                }

                // 🔎 ···│\n
                ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                ctx.Builder.Append(Character.VerticalLine, Color.Grey);
                ctx.Builder.CommitLine();

                // Got labels with notes?
                var labelsWithNotes = group.Labels.Where(l => l.Note != null).ToArray();
                if (labelsWithNotes != null)
                {
                    foreach (var (_, firstLabel, lastLabel, labelWithNote) in labelsWithNotes.Enumerate())
                    {
                        if (firstLabel)
                        {
                            // 🔎 ···(dot)\n
                            ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                            ctx.Builder.Append(Character.Dot, Color.Grey);
                            ctx.Builder.CommitLine();
                        }

                        // Got a note?
                        if (!string.IsNullOrWhiteSpace(labelWithNote.Note))
                        {
                            // 🔎 ···(dot) NOTE: This is a note\n
                            ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                            ctx.Builder.Append(Character.Dot, Color.Grey);
                            ctx.Builder.AppendSpace();
                            ctx.Builder.Append("NOTE: ", Color.Aqua);
                            ctx.Builder.Append(labelWithNote.Note ?? string.Empty);
                            ctx.Builder.CommitLine();
                        }

                        if (lastLabel)
                        {
                            // 🔎 ···(dot)\n
                            ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                            ctx.Builder.Append(Character.Dot, Color.Grey);
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

        private static void RenderHorizontalLines(
            DiagnosticContext ctx, TextLine line,
            IReadOnlyList<LineLabel> labels,
            int row, LineLabel label)
        {
            RenderMargin(ctx, line, false);
            ctx.Builder.AppendSpace();

            for (var col = 0; col < line.Text.Length; col++)
            {
                var vbar = GetVerticalLine(labels, col, row);
                var hbar = !string.IsNullOrWhiteSpace(label.Label.Message)
                    && label.ShouldRenderMessage
                    && col > label.Anchor;

                if (col == label.Anchor && !string.IsNullOrWhiteSpace(label.Label.Message))
                {
                    // 🔎 ╰
                    ctx.Builder.Append(Character.BottomLeftCornerRound, label.Label.Color);
                }
                else if (vbar != null && (col != label.Anchor || !string.IsNullOrWhiteSpace(label.Label.Message)))
                {
                    if (hbar)
                    {
                        // 🔎 ─
                        ctx.Builder.Append(Character.HorizontalLine, label.Label.Color);
                    }
                    else
                    {
                        // 🔎 │
                        ctx.Builder.Append(Character.VerticalLine, vbar.Label.Color);
                    }
                }
                else if (hbar)
                {
                    // 🔎 ─
                    ctx.Builder.Append(Character.HorizontalLine, label.Label.Color);
                }
                else
                {
                    // 🔎 ·
                    ctx.Builder.Append(' ', label.Label.Color);
                }
            }
        }

        private static void RenderVerticalLines(
            DiagnosticContext ctx, TextLine line,
            IReadOnlyList<LineLabel> labels, int row)
        {
            // Draw vertical lines
            RenderMargin(ctx, line, false);
            ctx.Builder.AppendSpace();

            for (var col = 0; col < line.Text.Length; col++)
            {
                var vbar = GetVerticalLine(labels, col, row);
                var underline = GetUnderlineColor(labels, line, col);

                if (row != 0)
                {
                    underline = null;
                }

                if (vbar != null)
                {
                    if (underline != null)
                    {
                        // 🔎 ┬
                        ctx.Builder.Append(Character.Anchor, underline);
                    }
                    else
                    {
                        // 🔎 │
                        ctx.Builder.Append(Character.AnchorVerticalLine, vbar.Label.Color);
                    }
                }
                else if (underline != null)
                {
                    // 🔎 ─
                    ctx.Builder.Append(Character.AnchorHorizontalLine, underline);
                }
                else
                {
                    // 🔎 ·
                    ctx.Builder.Append(' ');
                }
            }

            ctx.Builder.CommitLine();
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

        private static LineLabel? GetVerticalLine(IEnumerable<LineLabel> labels, int column, int row)
        {
            foreach (var (index, label) in labels.EnumerateWithIndex())
            {
                if (string.IsNullOrWhiteSpace(label.Label.Message))
                {
                    continue;
                }

                if (label.Anchor == column)
                {
                    if ((row <= index && !label.IsMultiLine) || (row <= index && label.IsMultiLine))
                    {
                        return label;
                    }
                }
            }

            return null;
        }

        private static Color? GetUnderlineColor(IEnumerable<LineLabel> labels, TextLine line, int column)
        {
            return labels.Where(label => !label.IsMultiLine && label.Label.SourceSpan.Contains(line.Offset + column))
                .OrderBy(l => l.Priority)
                .ThenBy(l => l.Columns.Start)
                .FirstOrDefault()?.Label?.Color;
        }

        private static Color? GetHighlightColor(IEnumerable<LineLabel> labels, TextLine line, int column)
        {
            return labels.Where(label => label.Label.SourceSpan.Contains(line.Offset + column))
                .OrderBy(l => l.Priority)
                .ThenBy(l => l.Columns.Start)
                .FirstOrDefault()?.Label?.Color;
        }
    }
}