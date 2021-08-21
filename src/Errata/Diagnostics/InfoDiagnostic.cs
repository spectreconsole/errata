using System;
using Spectre.Console;

namespace Errata
{
    [Obsolete("Use Diagnostic.Info(..) instead")]
    public sealed class InfoDiagnostic : Diagnostic
    {
        public InfoDiagnostic(string message)
            : base(message)
        {
            Color = Color.Blue;
            Category = "Info";
        }
    }
}
