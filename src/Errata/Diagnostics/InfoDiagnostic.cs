using System;
using Spectre.Console;

namespace Errata
{
    /// <summary>
    /// A diagnostic representing some kind of information.
    /// </summary>
    [Obsolete("Use Diagnostic.Info(..) instead")]
    public sealed class InfoDiagnostic : Diagnostic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InfoDiagnostic"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InfoDiagnostic(string message)
            : base(message)
        {
            Color = Color.Blue;
            Category = "Info";
        }
    }
}
