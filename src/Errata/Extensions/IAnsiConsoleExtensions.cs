using System;
using Spectre.Console;

namespace Errata
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static class IAnsiConsoleExtensions
    {
        /// <summary>
        /// Renders the report.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="report">The report.</param>
        /// <param name="settings">The settings.</param>
        public static void Render(this IAnsiConsole console, Report report, ReportSettings? settings = null)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (report is null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            report.Render(console, settings);
        }
    }
}
