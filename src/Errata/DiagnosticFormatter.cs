using System.Text;
using Spectre.Console;

namespace Errata
{
    public class DiagnosticFormatter
    {
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
