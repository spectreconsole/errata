using System;
using System.Collections.Generic;
using Spectre.Console;

namespace Errata
{
    public class Diagnostic
    {
        public string Message { get; }
        public List<Label> Labels { get; }
        public string? Category { get; set; }
        public string? Code { get; set; }
        public Color Color { get; set; }
        public string? Note { get; set; }

        public Diagnostic(string message)
        {
            Message = message;
            Labels = new List<Label>();
        }

        public Diagnostic WithColor(Color color)
        {
            Color = color;
            return this;
        }

        public Diagnostic WithCategory(string category)
        {
            Category = category;
            return this;
        }

        public Diagnostic WithCode(string code)
        {
            Code = code;
            return this;
        }

        public Diagnostic WithNote(string note)
        {
            Note = note;
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

        public static Diagnostic Warning(string message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return new Diagnostic(message)
                .WithCategory("Warning")
                .WithColor(Color.Yellow);
        }

        public static Diagnostic Error(string message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return new Diagnostic(message)
                .WithCategory("Error")
                .WithColor(Color.Red);
        }

        public static Diagnostic Info(string message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return new Diagnostic(message)
                .WithCategory("Info")
                .WithColor(Color.Blue);
        }
    }
}
