using System;

namespace Errata
{
    public static class ReportExtensions
    {
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
