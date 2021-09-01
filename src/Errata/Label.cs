using System;
using Spectre.Console;

namespace Errata
{
    /// <summary>
    /// Represents a label that is part of a diagnostic.
    /// </summary>
    public sealed class Label
    {
        private readonly TextSpan? _span;
        private readonly Location? _location;
        private int? _length;

        /// <summary>
        /// Gets the source ID.
        /// </summary>
        public string SourceId { get; }

        /// <summary>
        /// Gets the label message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets or sets the label color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the label note.
        /// </summary>
        public string? Note { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public int Priority { get; set; } = 0;

#if NET5_0_OR_GREATER
        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="sourceId">The source ID.</param>
        /// <param name="span">The character span in the source.</param>
        /// <param name="message">The message.</param>
        public Label(string sourceId, Range span, string message)
        {
            _span = new TextSpan(span);

            SourceId = sourceId ?? throw new ArgumentNullException(nameof(sourceId));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Color = Color.White;
        }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="sourceId">The source ID.</param>
        /// <param name="span">The character span in the source.</param>
        /// <param name="message">The message.</param>
        public Label(string sourceId, TextSpan span, string message)
        {
            _span = span;

            SourceId = sourceId ?? throw new ArgumentNullException(nameof(sourceId));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Color = Color.White;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="sourceId">The source ID.</param>
        /// <param name="location">The source location.</param>
        /// <param name="message">The message.</param>
        public Label(string sourceId, Location location, string message)
        {
            _location = location;
            _length = 1;

            SourceId = sourceId ?? throw new ArgumentNullException(nameof(sourceId));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Color = Color.White;
        }

        /// <summary>
        /// Sets the number of character this label occupies.
        /// </summary>
        /// <param name="length">The number of characters this label occupies.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Label WithLength(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than zero");
            }

            _length = length;
            return this;
        }

        internal TextSpan GetSourceSpan(Source source)
        {
            if (_span != null)
            {
                if (_length != null)
                {
                    return new TextSpan(_span.Value.Start, _span.Value.Start + _length.Value);
                }

                return _span.Value;
            }

            // Should not occur, but let's make sure anyway
            if (_location == null || _length == null)
            {
                throw new InvalidOperationException("Location info for label has not been set");
            }

            return source.GetSourceSpan(_location.Value, _length.Value);
        }
    }
}
