using System;

namespace Errata
{
    public struct Location : IEquatable<Location>
    {
        public int Row { get; }
        public int Column { get; }

        public Location(int row, int column)
        {
            if (row < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Row must be greater than zero");
            }
            else if (column < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "Column must be greater than zero");
            }

            Row = row;
            Column = column;
        }

        public bool Equals(Location other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object? obj)
        {
            return obj is Location location && Equals(location);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public static bool operator ==(Location left, Location right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Location left, Location right)
        {
            return !(left == right);
        }
    }
}
