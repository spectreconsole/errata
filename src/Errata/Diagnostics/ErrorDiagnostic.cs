using System;
using Spectre.Console;

namespace Errata
{
    [Obsolete("Use Diagnostic.Error(..) instead")]
    public sealed class ErrorDiagnostic : Diagnostic
    {
        public ErrorDiagnostic(string message)
            : base(message)
        {
            Color = Color.Red;
            Category = "Error";
        }
    }
}
