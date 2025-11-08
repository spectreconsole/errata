using System;
using Spectre.Console;

namespace Errata;

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
    public static Label WithNote(this Label label, string? note)
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

    /// <summary>
    /// Sets the number of additional context lines to display above this label.
    /// </summary>
    /// <param name="label">The label.</param>
    /// <param name="lines">The number of lines above the label to display as context.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Label WithContextLines(this Label label, int lines)
    {
        if (label is null)
        {
            throw new ArgumentNullException(nameof(label));
        }

        if (lines < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lines), "Context lines must be greater than or equal to zero");
        }

        label.ContextLines = lines;
        return label;
    }
}
