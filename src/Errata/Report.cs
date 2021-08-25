using System;
using System.Collections.Generic;
using Spectre.Console;

namespace Errata
{
    /// <summary>
    /// Represents a report.
    /// </summary>
    public sealed class Report
    {
        /// <summary>
        /// Gets the source repository.
        /// </summary>
        public ISourceRepository Repository { get; }

        /// <summary>
        /// Gets the diagnostics part of the report.
        /// </summary>
        public List<Diagnostic> Diagnostics { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Report"/> class.
        /// </summary>
        /// <param name="repository">The source repository.</param>
        public Report(ISourceRepository repository)
        {
            Diagnostics = new List<Diagnostic>();
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Renders the report to the specified console.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="settings">The settings.</param>
        public void Render(IAnsiConsole console, ReportSettings? settings = null)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            var renderer = new ReportRenderer(console, Repository);
            renderer.Render(this, settings);
        }
    }
}
