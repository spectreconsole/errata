namespace Errata
{
    /// <summary>
    /// The settings that should be used when rendering a <see cref="Report"/>.
    /// </summary>
    public sealed class ReportSettings
    {
        /// <summary>
        /// Gets or sets the character set.
        /// </summary>
        public CharacterSet? Characters { get; set; }

        /// <summary>
        /// Gets or sets the diagnostic formatter.
        /// </summary>
        public DiagnosticFormatter? Formatter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not
        /// the report should be rendered in compact mode.
        /// </summary>
        public bool Compact { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not exceptions
        /// should get propagated to the caller if rendering would fail.
        /// If set to <see langword="false" />, Errata errors will be
        /// rendered as part of the report.
        /// </summary>
        public bool PropagateExceptions { get; set; }

        internal bool ExcludeStackTrace { get; set; }
    }
}
