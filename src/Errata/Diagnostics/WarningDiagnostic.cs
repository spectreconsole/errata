using System;
using Spectre.Console;

namespace Errata
{
    /// <summary>
    /// A diagnostic representing a warning.
    /// </summary>
    [Obsolete("Use Diagnostic.Warning(..) instead")]
    public sealed class WarningDiagnostic : Diagnostic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WarningDiagnostic"/> class.
        /// </summary>
        /// <param name="message">The warning message.</param>
        public WarningDiagnostic(string message)
            : base(message)
        {
            Color = Color.Yellow;
            Category = "Warning";
        }
    }
}
