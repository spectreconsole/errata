using System;
using System.Collections.Generic;
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
                    var message = "[red]An error occured when rendering diagnostic[/]\n" +
                        "Error: " + ex.Message + "\n\n" +
                        "If you belive this is a bug in Errata, please submit it\n" +
                        "at https://github.com/spectreconsole/errata/issues/new";

                    errors.Add(
                        new Panel(message)
                            .Header("Errata Error")
                            .BorderColor(Color.Red));
                    errors.Add(new Text("\n"));
                }
            }

            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    _console.Write(error);
                }
            }

            _console.Write(new ReportRenderable(ctx.Builder.GetLines()));
        }
    }
}
