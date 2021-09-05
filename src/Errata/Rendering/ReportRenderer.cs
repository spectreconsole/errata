using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;

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

            var ctx = new ReportContext(_console, _repository, settings);
            var renderer = new DiagnosticRenderer(ctx);

            var errors = new List<IRenderable>();

            foreach (var (_, first, _, diagnostic) in report.Diagnostics.Enumerate())
            {
                if (!first)
                {
                    // Add space between diagnostics
                    ctx.Builder.CommitLine();
                }

                try
                {
                    renderer.Render(diagnostic);
                }
                catch (Exception ex) when (!ctx.PropagateExceptions)
                {
                    errors.Add(GetError(ctx, diagnostic, ex));
                    errors.Add(new Text("\n"));
                }
            }

            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    _console.Write(error);
                }

                errors.Add(new Text("\n"));
            }

            _console.Write(new ReportRenderable(ctx.Builder.GetLines()));
        }

        private static IRenderable GetError(ReportContext ctx, Diagnostic diagnostic, Exception ex)
        {
            var builder = new StringBuilder();
            builder.AppendLine("An error occured when rendering a diagnostic.");
            builder.Append("Message: [red]").Append(ex.Message.EscapeMarkup()).AppendLine("[/]");

            // Diagnostics
            builder.AppendLine();
            builder.AppendLine("[blue]Diagnostic:[/]");
            builder.Append("Message").Append(": [yellow]").Append(diagnostic.Message.EscapeMarkup()).AppendLine("[/]");

            if (diagnostic.Category != null)
            {
                builder.Append("Category").Append(": [yellow]").Append(diagnostic.Category.EscapeMarkup()).AppendLine("[/]");
            }

            if (diagnostic.Code != null)
            {
                builder.Append("Code").Append(": [yellow]").Append(diagnostic.Code.EscapeMarkup()).AppendLine("[/]");
            }

            if (diagnostic.Note != null)
            {
                builder.Append("Note").Append(": [yellow]").Append(diagnostic.Note.EscapeMarkup()).AppendLine("[/]");
            }

            var sources = string.Join("[silver],[/] ", diagnostic.Labels.Select(l => l.SourceId.EscapeMarkup()));
            if (!string.IsNullOrWhiteSpace(sources))
            {
                var key = diagnostic.Labels.Count == 1 ? "Source" : "Sources";
                builder.Append(key).Append(": [yellow]").Append(sources).AppendLine("[/]");
            }

            // Context
            if (ex is ErrataException errataException && errataException.Context.Count > 0)
            {
                builder.AppendLine();
                builder.AppendLine("[blue]Context:[/]");

                foreach (var item in errataException.Context)
                {
                    builder.Append(item.Key.EscapeMarkup())
                        .Append(": [yellow]")
                        .Append(item.Value?.ToString()?.EscapeMarkup() ?? string.Empty)
                        .AppendLine("[/]");
                }
            }

            if (!string.IsNullOrWhiteSpace(ex.StackTrace) && !ctx.ExcludeStackTrace)
            {
                builder.AppendLine();
                builder.AppendLine("[blue]Stack trace:[/]");
                builder.AppendLine(ex.StackTrace);
            }

            // Help link
            builder.AppendLine();
            builder.Append("If you believe this is a bug in Errata, please submit an issue " +
                "at [yellow link]https://github.com/spectreconsole/errata/issues/new[/]");

            return new Panel(builder.ToString()).Header("Errata Error").BorderColor(Color.Red);
        }
    }
}
