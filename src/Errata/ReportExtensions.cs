using System;

namespace Errata
{
    /// <summary>
    /// Contains extension methods for <see cref="Report"/>.
    /// </summary>
    public static class ReportExtensions
    {
        /// <summary>
        /// Adds a diagnostic to the report.
        /// </summary>
        /// <typeparam name="T">The diagnostic type.</typeparam>
        /// <param name="report">The report.</param>
        /// <param name="diagnostic">The diagnostic to add.</param>
        /// <returns>The provided diagnostic instance so that multiple calls can be chained.</returns>
        public static T AddDiagnostic<T>(this Report report, T diagnostic)
            where T : Diagnostic
        {
            if (report is null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            if (diagnostic is null)
            {
                throw new ArgumentNullException(nameof(diagnostic));
            }

            report.Diagnostics.Add(diagnostic);
            return diagnostic;
        }
    }
}
