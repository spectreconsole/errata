using System;
using Spectre.Console;

namespace Errata
{
    public sealed class Label
    {
        private readonly Range? _range;
        private readonly Location? _location;
        private int? _length;

        public string SourceId { get; }
        public string Message { get; }
        public Color Color { get; set; }
        public string? Note { get; set; }

        public Label(string sourceId, Range range, string message)
        {
            _range = range;

            SourceId = sourceId ?? throw new ArgumentNullException(nameof(sourceId));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Color = Color.White;
        }

        public Label(string sourceId, Location location, string message)
        {
            _location = location;
            _length = 1;

            SourceId = sourceId ?? throw new ArgumentNullException(nameof(sourceId));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Color = Color.White;
        }

        internal Range GetSpan(Source source)
        {
            if (_range != null)
            {
                if (_length != null)
                {
                    return _range.Value.Start..(_range.Value.Start.Value + _length.Value);
                }

                return _range.Value;
            }

            // Should not occur, but let's make sure anyway
            if (_location == null || _length == null)
            {
                throw new InvalidOperationException("Location info for label has not been set");
            }

            return source.GetSpan(_location.Value, _length.Value);
        }

        public Label WithLength(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than zero");
            }

            _length = length;
            return this;
        }

        public Label WithColor(Color color)
        {
            Color = color;
            return this;
        }

        public Label WithNote(string note)
        {
            if (note is null)
            {
                throw new ArgumentNullException(nameof(note));
            }

            Note = note;
            return this;
        }
    }
}
