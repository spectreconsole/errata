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
            _console = console ?? throw new ArgumentNullException(nameof(console));
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

        private void Render(ReportRendererContext builder, Diagnostic diagnostic)
        {
            // Create the source groups from the labels
            var groups = SourceGroupCollection.CreateFromLabels(_repository, diagnostic.Labels);

            // 🔎 Error [ABC123]: This is the error
            var prefix = diagnostic.GetPrefix();
            builder.Append(prefix, diagnostic.GetColor());
            builder.Append(Character.Colon);
            builder.Append(' ');
            builder.Append(diagnostic.Message);
            builder.CommitLine();

            if (!string.IsNullOrWhiteSpace(diagnostic.Note))
            {
                // 🔎 NOTE: This is a note
                builder.Append("NOTE: ", Color.Aqua);
                builder.Append(diagnostic.Note);
                builder.CommitLine();
            }

            // Find out the max width of the line number
            var lineNumberMaxWidth = groups.GetLineNumberMaxWidth();

            // Iterate all source groups
            foreach (var (_, first, last, group) in groups.Enumerate())
            {
                var lineRange = group.Source.GetLineSpan(group.Span);

                // 🔎 ···┌─[Program.cs]
                builder.AppendSpaces(lineNumberMaxWidth + 2);
                builder.Append(first ? Character.TopLeftCornerHard : Character.LeftConnector, Color.Grey);
                builder.Append(Character.HorizontalLine, Color.Grey);
                builder.Append("[", Color.Grey);
                builder.Append(group.Source.Name, Color.White);
                builder.Append("]", Color.Grey);
                builder.CommitLine();

                // 🔎 ···│
                builder.AppendSpaces(lineNumberMaxWidth + 2);
                builder.Append(Character.VerticalLine, Color.Grey);
                builder.CommitLine();

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
                    builder.Append($" {(lineIndex + 1).ToString().PadRight(lineNumberMaxWidth)} ");
                    builder.Append(Character.VerticalLine, Color.Grey);
                    builder.Append(" ");
                    builder.Append(line.Text);
                    builder.CommitLine();

                    LineRenderer.DrawAnchors(builder, labels, lineNumberMaxWidth);
                    LineRenderer.DrawLines(builder, labels, lineNumberMaxWidth);

                    if (lineIndex != lineRange.End.Value)
                    {
                        // 🔎 ···(dot)
                        builder.AppendSpaces(lineNumberMaxWidth + 2);
                        builder.Append(Character.Dot, Color.Grey);
                        builder.Append(" ");
                        builder.CommitLine();
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
                            builder.AppendSpaces(lineNumberMaxWidth + 2);
                            builder.Append(Character.Dot, Color.Grey);
                            builder.CommitLine();
                        }

                        // Got a note?
                        if (!string.IsNullOrWhiteSpace(labelWithNote.Note))
                        {
                            // 🔎 ···(dot) NOTE: This is a note
                            builder.AppendSpaces(lineNumberMaxWidth + 2);
                            builder.Append(Character.Dot, Color.Grey);
                            builder.AppendSpace();
                            builder.Append("NOTE: ", Color.Aqua);
                            builder.Append(labelWithNote.Note ?? string.Empty);
                            builder.CommitLine();
                        }

                        if (lastLabel)
                        {
                            // 🔎 ···(dot)
                            builder.AppendSpaces(lineNumberMaxWidth + 2);
                            builder.Append(Character.Dot, Color.Grey);
                            builder.CommitLine();
                        }
                    }
                }

                if (last)
                {
                    // 🔎 ···│
                    builder.AppendSpaces(lineNumberMaxWidth + 2);
                    builder.Append(Character.VerticalLine, Color.Grey);
                    builder.CommitLine();

                    // 🔎 ···└─
                    builder.AppendSpaces(lineNumberMaxWidth + 2);
                    builder.Append(Character.BottomLeftCornerHard, Color.Grey);
                    builder.Append(Character.HorizontalLine, Color.Grey);
                    builder.CommitLine();
                }
            }
        }
    }
}
