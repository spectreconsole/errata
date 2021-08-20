using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Errata
{
    public sealed class InMemorySourceRepository : ISourceRepository
    {
        private readonly Dictionary<string, Source> _lookup;

        public InMemorySourceRepository()
        {
            _lookup = new Dictionary<string, Source>(StringComparer.Ordinal);
        }

        public void Register(Source source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            _lookup[source.Id] = source;
        }

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

        public bool TryGet(string id, [NotNullWhen(true)] out Source? source)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return _lookup.TryGetValue(id, out source);
        }
    }
}
