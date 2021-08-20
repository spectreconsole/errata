using System;
using Spectre.Console;

namespace Errata
{
    public sealed class WarningDiagnostic : Diagnostic
    {
        public string? Code { get; set; }

        public WarningDiagnostic(string message)
            : base(message)
        {
        }

        public override Color GetColor()
        {
            return Color.Yellow;
        }

        public override string GetPrefix()
        {
            if (string.IsNullOrWhiteSpace(Code))
            {
                return "Warning";
            }

            return $"Warning [{Code}]";
        }

        public Diagnostic WithCode(string code)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            return this;
        }
    }
}
