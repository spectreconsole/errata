using System;
using Spectre.Console;

namespace Errata
{
    /// <summary>
    /// A diagnostic representing an error.
    /// </summary>
    [Obsolete("Use Diagnostic.Error(..) instead")]
    public sealed class ErrorDiagnostic : Diagnostic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDiagnostic"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public ErrorDiagnostic(string message)
            : base(message)
        {
            Color = Color.Red;
            Category = "Error";
        }
    }
}
