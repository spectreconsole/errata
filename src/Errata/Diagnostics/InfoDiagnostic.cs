using System;
using Spectre.Console;

namespace Errata
{
    public sealed class InfoDiagnostic : Diagnostic
    {
        public string? Code { get; set; }

        public InfoDiagnostic(string message)
            : base(message)
        {
        }

        public override Color GetColor()
        {
            return Color.Blue;
        }

        public override string GetPrefix()
        {
            if (string.IsNullOrWhiteSpace(Code))
            {
                return "Info";
            }

            return $"Info [{Code}]";
        }

        public Diagnostic WithCode(string code)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            return this;
        }
    }
}
