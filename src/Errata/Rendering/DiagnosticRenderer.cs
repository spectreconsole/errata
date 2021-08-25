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

            // 🔎 Error [ABC123]: This is the error
            var prefix = ctx.Formatter.Format(diagnostic);
            if (prefix != null)
            {
                ctx.Builder.AppendMarkup(prefix);
                ctx.Builder.CommitLine();
            }

            if (!string.IsNullOrWhiteSpace(diagnostic.Note))
            {
                // 🔎 NOTE: This is a note
                ctx.Builder.Append("NOTE: ", Color.Aqua);
                ctx.Builder.Append(diagnostic.Note);
                ctx.Builder.CommitLine();
            }

            // Iterate all source groups
            foreach (var (_, first, last, group) in ctx.Groups.Enumerate())
            {
                var lineRange = group.Source.GetLineSpan(group.Span);

                // 🔎 ···┌─[Program.cs]
                ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                ctx.Builder.Append(first ? Character.TopLeftCornerHard : Character.LeftConnector, Color.Grey);
                ctx.Builder.Append(Character.HorizontalLine, Color.Grey);
                ctx.Builder.Append("[", Color.Grey);
                ctx.Builder.Append(group.Source.Name, Color.White);
                ctx.Builder.Append("]", Color.Grey);
                ctx.Builder.CommitLine();

                // 🔎 ···│
                ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                ctx.Builder.Append(Character.VerticalLine, Color.Grey);
                ctx.Builder.CommitLine();

                // Iterate all lines
                for (var lineIndex = lineRange.Start.Value; lineIndex <= lineRange.End.Value; lineIndex++)
                {
                    // Get the current line and it's labels
                    var line = group.Source.Lines[lineIndex];
                    var labels = group.GetLabelsForLine(line);
                    if (labels.Count == 0)
                    {
                        continue;
                    }

                    // 🔎 ·38·│ var foo = bar
                    RenderMargin(ctx, lineNumber: lineIndex + 1);
                    ctx.Builder.AppendSpace();
                    ctx.Builder.Append(line.Text);
                    ctx.Builder.CommitLine();

                    LineRenderer.DrawAnchors(ctx, labels);
                    LineRenderer.DrawLines(ctx, labels);

                    if (lineIndex != lineRange.End.Value)
                    {
                        // 🔎 ···(dot)
                        RenderMargin(ctx);
                        ctx.Builder.CommitLine();
                    }
                }

                // Got labels with notes?
                var labelsWithNotes = group.Labels.Where(l => l.Note != null).ToArray();
                if (labelsWithNotes != null)
                {
                    foreach (var (_, firstLabel, lastLabel, labelWithNote) in labelsWithNotes.Enumerate())
                    {
                        if (firstLabel)
                        {
                            // 🔎 ···(dot)
                            RenderMargin(ctx);
                            ctx.Builder.CommitLine();
                        }

                        // Got a note?
                        if (!string.IsNullOrWhiteSpace(labelWithNote.Note))
                        {
                            // 🔎 ···(dot) NOTE: This is a note
                            RenderMargin(ctx);
                            ctx.Builder.AppendSpace();
                            ctx.Builder.Append("NOTE: ", Color.Aqua);
                            ctx.Builder.Append(labelWithNote.Note ?? string.Empty);
                            ctx.Builder.CommitLine();
                        }

                        if (lastLabel)
                        {
                            // 🔎 ···(dot)
                            RenderMargin(ctx);
                            ctx.Builder.CommitLine();
                        }
                    }
                }

                if (last)
                {
                    // 🔎 ···│
                    ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                    ctx.Builder.Append(Character.VerticalLine, Color.Grey);
                    ctx.Builder.CommitLine();

                    // 🔎 ···└─
                    ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                    ctx.Builder.Append(Character.BottomLeftCornerHard, Color.Grey);
                    ctx.Builder.Append(Character.HorizontalLine, Color.Grey);
                    ctx.Builder.CommitLine();
                }
            }
        }

        private static void RenderMargin(DiagnosticContext ctx, int? lineNumber = null)
        {
            if (lineNumber == null)
            {
                // 🔎 ···(dot)
                ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                ctx.Builder.Append(Character.Dot, Color.Grey);
            }
            else
            {
                ctx.Builder.AppendSpace();
                ctx.Builder.Append(lineNumber.Value.ToString().PadRight(ctx.LineNumberWidth));
                ctx.Builder.AppendSpace();
                ctx.Builder.Append(Character.VerticalLine, Color.Grey);
            }
        }
    }
}