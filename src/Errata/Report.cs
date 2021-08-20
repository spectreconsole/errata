using System;
using System.Collections.Generic;
using Spectre.Console;

namespace Errata
{
    public sealed class Report
    {
        public List<Diagnostic> Diagnostics { get; }

        public Report()
        {
            Diagnostics = new List<Diagnostic>();
        }

        public void Render(IAnsiConsole console, ISourceRepository repository, ReportSettings? settings = null)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            var renderer = new ReportRenderer(console, repository);
            renderer.Render(this, settings);
        }
    }
}
