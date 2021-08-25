using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Errata
{
    /// <summary>
    /// Represents an in-memory source repository.
    /// </summary>
    public sealed class InMemorySourceRepository : ISourceRepository
    {
        private readonly Dictionary<string, Source> _lookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemorySourceRepository"/> class.
        /// </summary>
        public InMemorySourceRepository()
        {
            _lookup = new Dictionary<string, Source>(StringComparer.Ordinal);
        }

        /// <summary>
        /// Registers the specified source with the repository.
        /// </summary>
        /// <param name="source">The source.</param>
        public void Register(Source source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            _lookup[source.Id] = source;
        }

        /// <summary>
        /// Registers the specified ID and source text with the repository.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="source">The source text.</param>
        public void Register(string id, string source)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            _lookup[id] = new Source(id, source);
        }

        /// <inheritdoc/>
        bool ISourceRepository.TryGet(string id, [NotNullWhen(true)] out Source? source)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return _lookup.TryGetValue(id, out source);
        }
    }
}
