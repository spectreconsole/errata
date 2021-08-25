using System.Text;
using Spectre.Console;

namespace Errata
{
    /// <summary>
    /// A formatter for diagnostics.
    /// </summary>
    public class DiagnosticFormatter
    {
        /// <summary>
        /// Gets a markup representation of the header for a diagnostic.
        /// </summary>
        /// <param name="diagnostic">The diagnostic.</param>
        /// <returns>A markup representation of the header for a diagnostic.</returns>
        public virtual Markup? Format(Diagnostic diagnostic)
        {
            var builder = new StringBuilder();

            if (diagnostic.Category != null)
            {
                builder.Append("[b]")
                    .Append(diagnostic.Category.EscapeMarkup())
                    .Append("[/]");
            }

            if (diagnostic.Code != null)
            {
                if (diagnostic.Category != null)
                {
                    builder.Append(' ');
                }

                builder.Append("[[");
                builder.Append(diagnostic.Code.EscapeMarkup());
                builder.Append("]]");
            }

            if (!string.IsNullOrWhiteSpace(diagnostic.Category)
                || !string.IsNullOrWhiteSpace(diagnostic.Code))
            {
                builder.Append("[white]: [/]");
            }

            builder.Append("[white]")
                .Append(diagnostic.Message.EscapeMarkup())
                .Append("[/]");

            return new Markup(
                builder.ToString(),
                new Style(diagnostic.Color));
        }
    }
}
