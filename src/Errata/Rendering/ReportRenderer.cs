using System;
using System.Linq;
using Spectre.Console;

namespace Errata
{
    internal sealed class ReportRenderer
    {
        private readonly IAnsiConsole _console;
        private readonly ISourceRepository _repository;

        public ReportRenderer(IAnsiConsole console, ISourceRepository repository)
        {
            _console = new RenderConsole(console);
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Render(Report report, ReportSettings? settings)
        {
            if (report is null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            var ctx = new ReportRendererContext(_console, settings);
            foreach (var (_, first, _, diagnostic) in report.Diagnostics.Enumerate())
            {
                if (!first)
                {
                    ctx.CommitLine();
                }

                Render(ctx, diagnostic);
            }

            _console.Write(new ReportRenderable(ctx.GetLines()));
        }

        private void Render(ReportRendererContext ctx, Diagnostic diagnostic)
        {
            // Create the source groups from the labels
            var groups = SourceGroupCollection.CreateFromLabels(_repository, diagnostic.Labels);

            // 🔎 Error [ABC123]: This is the error
            var prefix = ctx.Formatter.Format(diagnostic);
            if (prefix != null)
            {
                ctx.AppendMarkup(prefix);
                ctx.CommitLine();
            }

            if (!string.IsNullOrWhiteSpace(diagnostic.Note))
            {
                // 🔎 NOTE: This is a note
                ctx.Append("NOTE: ", Color.Aqua);
                ctx.Append(diagnostic.Note);
                ctx.CommitLine();
            }

            // Find out the max width of the line number
            var lineNumberMaxWidth = groups.GetLineNumberMaxWidth();

            // Iterate all source groups
            foreach (var (_, first, last, group) in groups.Enumerate())
            {
                var lineRange = group.Source.GetLineSpan(group.Span);

                // 🔎 ···┌─[Program.cs]
                ctx.AppendSpaces(lineNumberMaxWidth + 2);
                ctx.Append(first ? Character.TopLeftCornerHard : Character.LeftConnector, Color.Grey);
                ctx.Append(Character.HorizontalLine, Color.Grey);
                ctx.Append("[", Color.Grey);
                ctx.Append(group.Source.Name, Color.White);
                ctx.Append("]", Color.Grey);
                ctx.CommitLine();

                // 🔎 ···│
                ctx.AppendSpaces(lineNumberMaxWidth + 2);
                ctx.Append(Character.VerticalLine, Color.Grey);
                ctx.CommitLine();

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
                    ctx.Append($" {(lineIndex + 1).ToString().PadRight(lineNumberMaxWidth)} ");
                    ctx.Append(Character.VerticalLine, Color.Grey);
                    ctx.Append(" ");
                    ctx.Append(line.Text);
                    ctx.CommitLine();

                    LineRenderer.DrawAnchors(ctx, labels, lineNumberMaxWidth);
                    LineRenderer.DrawLines(ctx, labels, lineNumberMaxWidth);

                    if (lineIndex != lineRange.End.Value)
                    {
                        // 🔎 ···(dot)
                        ctx.AppendSpaces(lineNumberMaxWidth + 2);
                        ctx.Append(Character.Dot, Color.Grey);
                        ctx.Append(" ");
                        ctx.CommitLine();
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
                            ctx.AppendSpaces(lineNumberMaxWidth + 2);
                            ctx.Append(Character.Dot, Color.Grey);
                            ctx.CommitLine();
                        }

                        // Got a note?
                        if (!string.IsNullOrWhiteSpace(labelWithNote.Note))
                        {
                            // 🔎 ···(dot) NOTE: This is a note
                            ctx.AppendSpaces(lineNumberMaxWidth + 2);
                            ctx.Append(Character.Dot, Color.Grey);
                            ctx.AppendSpace();
                            ctx.Append("NOTE: ", Color.Aqua);
                            ctx.Append(labelWithNote.Note ?? string.Empty);
                            ctx.CommitLine();
                        }

                        if (lastLabel)
                        {
                            // 🔎 ···(dot)
                            ctx.AppendSpaces(lineNumberMaxWidth + 2);
                            ctx.Append(Character.Dot, Color.Grey);
                            ctx.CommitLine();
                        }
                    }
                }

                if (last)
                {
                    // 🔎 ···│
                    ctx.AppendSpaces(lineNumberMaxWidth + 2);
                    ctx.Append(Character.VerticalLine, Color.Grey);
                    ctx.CommitLine();

                    // 🔎 ···└─
                    ctx.AppendSpaces(lineNumberMaxWidth + 2);
                    ctx.Append(Character.BottomLeftCornerHard, Color.Grey);
                    ctx.Append(Character.HorizontalLine, Color.Grey);
                    ctx.CommitLine();
                }
            }
        }
    }
}
