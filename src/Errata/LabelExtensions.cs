using System;
using Spectre.Console;

namespace Errata
{
    /// <summary>
    /// Contains extension methods for <see cref="Label"/>.
    /// </summary>
    public static class LabelExtensions
    {
        /// <summary>
        /// Sets the label color.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="color">The color.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Label WithColor(this Label label, Color color)
        {
            if (label is null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            label.Color = color;
            return label;
        }

        /// <summary>
        /// Sets the label note.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="note">The note.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Label WithNote(this Label label, string note)
        {
            if (label is null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            label.Note = note;
            return label;
        }

        /// <summary>
        /// Sets the label priority.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="priority">The label priority. Lower priority gets rendered first (vertically).</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Label WithPriority(this Label label, int priority)
        {
            if (label is null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            label.Priority = priority;
            return label;
        }
    }
}
