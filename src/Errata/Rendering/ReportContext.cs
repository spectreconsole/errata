using System;
using Spectre.Console;

namespace Errata
{
    internal sealed class ReportContext
    {
        private readonly IAnsiConsole _console;
        private readonly ReportSettings _settings;
        private readonly ISourceRepository _repository;

        public ReportBuilder Builder { get; }
        public CharacterSet Characters { get; }
        public DiagnosticFormatter Formatter { get; }
        public bool Compact { get; }
        public bool PropagateExceptions { get; }
        public bool ExcludeStackTrace { get; }

        public ReportContext(IAnsiConsole console, ISourceRepository repository, ReportSettings? settings)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _settings = settings ?? new ReportSettings();

            Characters = _settings.Characters ??= CharacterSet.Create(_console);
            Formatter = _settings.Formatter ?? new DiagnosticFormatter();
            Builder = new ReportBuilder(_console, Characters);
            Compact = _settings.Compact;
            PropagateExceptions = _settings.PropagateExceptions;
            ExcludeStackTrace = _settings.ExcludeStackTrace;
        }

        public DiagnosticContext CreateDiagnosticContext(Diagnostic diagnostic)
        {
            var groups = SourceGroupCollection.CreateFromLabels(_repository, diagnostic.Labels);
            return new DiagnosticContext(this, diagnostic, groups);
        }
    }
}
