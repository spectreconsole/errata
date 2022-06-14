using System;

namespace Errata;

/// <summary>
/// Represents a source location.
/// </summary>
public struct Location : IEquatable<Location>
{
    /// <summary>
    /// Gets the (zero indexed) row.
    /// </summary>
    public int Row { get; }

    /// <summary>
    /// Gets the (zero indexed) column.
    /// </summary>
    public int Column { get; }

    /// <summary>
    /// Gets a value indicating whether or not the location is a row.
    /// </summary>
    public bool IsRow { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Location"/> struct.
    /// </summary>
    /// <param name="row">The row.</param>
    public Location(int row)
    {
        if (row < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(row), "Row must be greater than zero");
        }

        Row = row;
        Column = 0;
        IsRow = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Location"/> struct.
    /// </summary>
    /// <param name="row">The row.</param>
    /// <param name="column">The column.</param>
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
        IsRow = false;
    }

    /// <inheritdoc/>
    public bool Equals(Location other)
    {
        return Row == other.Row && Column == other.Column;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Location location && Equals(location);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            var hash = (int)2166136261;
            hash = (hash * 16777619) ^ Row.GetHashCode();
            hash = (hash * 16777619) ^ Column.GetHashCode();
            return hash;
        }
    }

    /// <summary>
    /// Checks if two <see cref="Location"/> instances are equal.
    /// </summary>
    /// <param name="left">The first location instance to compare.</param>
    /// <param name="right">The second location instance to compare.</param>
    /// <returns><c>true</c> if the two location are equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(Location left, Location right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Checks if two <see cref="Location"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first location instance to compare.</param>
    /// <param name="right">The second location instance to compare.</param>
    /// <returns><c>true</c> if the two location are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(Location left, Location right)
    {
        return !(left == right);
    }
}
