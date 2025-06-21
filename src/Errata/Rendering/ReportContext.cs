using System;
using Spectre.Console;

namespace Errata;

internal sealed class ReportContext
{
    public ReportBuilder Builder { get; }
    public CharacterSet Characters { get; }
    public DiagnosticFormatter Formatter { get; }
    public ReportSettings Settings { get; }
    public ISourceRepository Repository { get; }

    public ReportContext(IAnsiConsole console, ISourceRepository repository, ReportSettings? settings)
    {
        if (console == null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        Settings = settings ?? new ReportSettings();
        Characters = Settings.Characters ??= CharacterSet.Create(console);
        Formatter = Settings.Formatter ?? new DiagnosticFormatter();
        Builder = new ReportBuilder(console, Characters);
    }

    public DiagnosticContext CreateDiagnosticContext(Diagnostic diagnostic)
    {
        var groups = SourceGroupCollection.CreateFromLabels(Repository, diagnostic.Labels);
        return new DiagnosticContext(this, diagnostic, groups);
    }
}
