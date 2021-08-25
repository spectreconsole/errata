using System;
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

            var ctx = new ReportContext(_console, _repository, settings);
            var renderer = new DiagnosticRenderer(ctx);

            foreach (var (_, first, _, diagnostic) in report.Diagnostics.Enumerate())
            {
                if (!first)
                {
                    // Add space between diagnostics
                    ctx.Builder.CommitLine();
                }

                renderer.Render(diagnostic);
            }

            _console.Write(new ReportRenderable(ctx.Builder.GetLines()));
        }
    }
}
