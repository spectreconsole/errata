using System.Diagnostics.CodeAnalysis;

namespace Errata
{
    /// <summary>
    /// Represents a source repository.
    /// </summary>
    public interface ISourceRepository
    {
        /// <summary>
        /// Tries to retrive a <see cref="Source"/> instance represented
        /// by the provided ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="source">The retrieved <see cref="Source"/> instance, or <c>null</c> if not present.</param>
        /// <returns><c>true</c> if the specified source instance was found, otherwise <c>false</c>.</returns>
        bool TryGet(string id, [NotNullWhen(true)] out Source? source);
    }
}
