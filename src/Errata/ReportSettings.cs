namespace Errata;

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
    /// <remarks>Defaults to <c>false</c>.</remarks>
    public bool Compact { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// the report should be padded on the left side.
    /// </summary>
    /// <remarks>Defaults to <c>true</c>.</remarks>
    public bool LeftPadding { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether or not exceptions
    /// should get propagated to the caller if rendering would fail.
    /// If set to <see langword="false" />, Errata errors will be
    /// rendered as part of the report.
    /// </summary>
    /// <remarks>Defaults to <c>false</c>.</remarks>
    public bool PropagateExceptions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// line numbers are included.
    /// </summary>
    /// <remarks>Defaults to <c>true</c>.</remarks>
    public bool ShowLineNumbers { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// the path is shown.
    /// </summary>
    /// <remarks>Defaults to <c>true</c>.</remarks>
    public bool ShowPath { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether or not stack traces
    /// should be excluded.
    /// </summary>
    internal bool ExcludeStackTrace { get; set; }
}
