using System;
using Spectre.Console;

namespace Errata
{
    [Obsolete("Use Diagnostic.Warning(..) instead")]
    public sealed class WarningDiagnostic : Diagnostic
    {
        public WarningDiagnostic(string message)
            : base(message)
        {
            Color = Color.Yellow;
            Category = "Warning";
        }
    }
}
