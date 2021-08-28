using System;

namespace Errata
{
    internal sealed class DiagnosticContext
    {
        private readonly ReportContext _ctx;

        public ReportBuilder Builder => _ctx.Builder;
        public DiagnosticFormatter Formatter => _ctx.Formatter;
        public CharacterSet Characters => _ctx.Characters;
        public bool Compact => _ctx.Compact;

        public Diagnostic Diagnostic { get; }
        public SourceGroupCollection Groups { get; }
        public int LineNumberWidth { get; }

        public DiagnosticContext(ReportContext ctx, Diagnostic diagnostic, SourceGroupCollection groups)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));

            Diagnostic = diagnostic ?? throw new ArgumentNullException(nameof(diagnostic));
            Groups = groups ?? throw new ArgumentNullException(nameof(groups));
            LineNumberWidth = groups.GetLineNumberMaxWidth();
        }
    }
}
