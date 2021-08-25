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
    }
}
