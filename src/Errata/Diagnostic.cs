using System;
using System.Collections.Generic;
using Spectre.Console;

namespace Errata
{
    /// <summary>
    /// Represents a diagnostic.
    /// </summary>
    public class Diagnostic
    {
        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the labels.
        /// </summary>
        public List<Label> Labels { get; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        public string? Note { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Diagnostic"/> class.
        /// </summary>
        /// <param name="message">The diagnostic message.</param>
        public Diagnostic(string message)
        {
            Message = message;
            Labels = new List<Label>();
        }

        /// <summary>
        /// Creates a diagnostic representing a warning.
        /// </summary>
        /// <param name="message">The warning message.</param>
        /// <returns>A diagnostic representing a warning.</returns>
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

        /// <summary>
        /// Creates a diagnostic representing an error.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <returns>A diagnostic representing an error.</returns>
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

        /// <summary>
        /// Creates a diagnostic representing information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A diagnostic representing information.</returns>
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
