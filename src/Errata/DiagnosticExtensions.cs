using System;
using Spectre.Console;

namespace Errata
{
    /// <summary>
    /// Contains extension methods for <see cref="Diagnostic"/>.
    /// </summary>
    public static class DiagnosticExtensions
    {
        /// <summary>
        /// Sets the diagnostic's color.
        /// </summary>
        /// <param name="diagnostic">The diagnostic.</param>
        /// <param name="color">The color to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Diagnostic WithColor(this Diagnostic diagnostic, Color color)
        {
            if (diagnostic is null)
            {
                throw new ArgumentNullException(nameof(diagnostic));
            }

            diagnostic.Color = color;
            return diagnostic;
        }

        /// <summary>
        /// Sets the diagnostic's category.
        /// </summary>
        /// <param name="diagnostic">The diagnostic.</param>
        /// <param name="category">The category to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Diagnostic WithCategory(this Diagnostic diagnostic, string category)
        {
            if (diagnostic is null)
            {
                throw new ArgumentNullException(nameof(diagnostic));
            }

            diagnostic.Category = category;
            return diagnostic;
        }

        /// <summary>
        /// Sets the diagnostic's code.
        /// </summary>
        /// <param name="diagnostic">The diagnostic.</param>
        /// <param name="code">The code to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Diagnostic WithCode(this Diagnostic diagnostic, string code)
        {
            if (diagnostic is null)
            {
                throw new ArgumentNullException(nameof(diagnostic));
            }

            diagnostic.Code = code;
            return diagnostic;
        }

        /// <summary>
        /// Sets the diagnostic's note.
        /// </summary>
        /// <param name="diagnostic">The diagnostic.</param>
        /// <param name="note">The note to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Diagnostic WithNote(this Diagnostic diagnostic, string note)
        {
            if (diagnostic is null)
            {
                throw new ArgumentNullException(nameof(diagnostic));
            }

            diagnostic.Note = note;
            return diagnostic;
        }

        /// <summary>
        /// Adds a label to the diagnostic.
        /// </summary>
        /// <param name="diagnostic">The diagnostic.</param>
        /// <param name="label">The label to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Diagnostic WithLabel(this Diagnostic diagnostic, Label label)
        {
            if (diagnostic is null)
            {
                throw new ArgumentNullException(nameof(diagnostic));
            }

            if (label is null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            diagnostic.Labels.Add(label);
            return diagnostic;
        }
    }
}
