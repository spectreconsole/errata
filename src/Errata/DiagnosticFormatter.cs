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
                builder.Append("[b]").Append(diagnostic.Category).Append("[/]");
            }

            if (diagnostic.Code != null)
            {
                if (diagnostic.Category != null)
                {
                    builder.Append(' ');
                }

                builder.Append("[[");
                builder.Append(diagnostic.Code);
                builder.Append("]]");
            }

            if (diagnostic.Category != null || diagnostic.Code != null)
            {
                builder.Append("[white]: [/]");
            }

            builder.Append("[white]").Append(diagnostic.Message).Append("[/]");

            return new Markup(
                builder.ToString(),
                new Style(diagnostic.Color));
        }
    }
}
