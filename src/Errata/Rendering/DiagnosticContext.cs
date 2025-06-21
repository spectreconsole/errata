using System;

namespace Errata;

internal sealed class DiagnosticContext
{
    private readonly ReportContext _ctx;

    public ReportBuilder Builder => _ctx.Builder;
    public DiagnosticFormatter Formatter => _ctx.Formatter;
    public CharacterSet Characters => _ctx.Characters;
    public bool Compact => _ctx.Settings.Compact;

    public Diagnostic Diagnostic { get; }
    public SourceGroupCollection Groups { get; }
    public int LineNumberWidth { get; }
    public bool HasLeftPadding => _ctx.Settings.LeftPadding;
    public int LeftPadding { get; }
    public bool ShowLineNumbers => _ctx.Settings.ShowLineNumbers;
    public bool ShowPath => _ctx.Settings.ShowPath;

    public DiagnosticContext(ReportContext ctx, Diagnostic diagnostic, SourceGroupCollection groups)
    {
        _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));

        Diagnostic = diagnostic ?? throw new ArgumentNullException(nameof(diagnostic));
        Groups = groups ?? throw new ArgumentNullException(nameof(groups));

        LineNumberWidth = ctx.Settings.ShowLineNumbers ? groups.GetLineNumberMaxWidth() : 0;
        LeftPadding = HasLeftPadding ? 2 : 1;

        if (!ShowLineNumbers && !HasLeftPadding)
        {
            LeftPadding = 0;
        }
    }
}
