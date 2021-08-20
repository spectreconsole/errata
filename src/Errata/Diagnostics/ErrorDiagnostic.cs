using System;
using Spectre.Console;

namespace Errata
{
    public sealed class ErrorDiagnostic : Diagnostic
    {
        public string? Code { get; set; }

        public ErrorDiagnostic(string message)
            : base(message)
        {
        }

        public override Color GetColor()
        {
            return Color.Red;
        }

        public override string GetPrefix()
        {
            if (string.IsNullOrWhiteSpace(Code))
            {
                return "Error";
            }

            return $"Error [{Code}]";
        }

        public Diagnostic WithCode(string code)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            return this;
        }
    }
}
