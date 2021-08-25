using System;

namespace Errata
{
    internal sealed class TextSpan
    {
        private readonly Range _range;

        public string SourceId { get; set; }
        public int Start => _range.Start.Value;
        public int End => _range.End.Value;
        public int Length => End - Start;

        public TextSpan(string sourceId, Range span)
        {
            SourceId = sourceId ?? throw new ArgumentNullException(nameof(sourceId));
            _range = span;
        }

        public bool Contains(int offset)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be equal or greater than zero (0)");
            }

            return Start <= offset && End > offset;
        }

        internal int LastOffset()
        {
            return Math.Max(_range.End.Value, _range.Start.Value);
        }
    }
}
