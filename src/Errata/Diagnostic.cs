using System;
using System.Collections.Generic;
using Spectre.Console;

namespace Errata
{
    public abstract class Diagnostic
    {
        public string Message { get; }
        public string? Note { get; set; }
        public List<Label> Labels { get; }

        protected Diagnostic(string message)
        {
            Message = message;
            Labels = new List<Label>();
        }

        public abstract Color GetColor();
        public abstract string GetPrefix();

        public Diagnostic WithNote(string note)
        {
            Note = note ?? throw new ArgumentNullException(nameof(note));
            return this;
        }

        public Diagnostic WithLabel(Label label)
        {
            if (label is null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            Labels.Add(label);
            return this;
        }

        public static WarningDiagnostic Warning(string message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return new WarningDiagnostic(message);
        }

        public static ErrorDiagnostic Error(string message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return new ErrorDiagnostic(message);
        }

        public static InfoDiagnostic Info(string message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return new InfoDiagnostic(message);
        }
    }
}
