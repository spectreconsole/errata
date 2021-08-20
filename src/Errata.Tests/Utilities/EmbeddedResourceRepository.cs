using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Errata.Tests
{
    public sealed class EmbeddedResourceRepository : ISourceRepository
    {
        private readonly Dictionary<string, Source> _lookup;

        public EmbeddedResourceRepository()
        {
            _lookup = new Dictionary<string, Source>(StringComparer.OrdinalIgnoreCase);
        }

        public bool TryGet(string id, [NotNullWhen(true)] out Source source)
        {
            if (!_lookup.TryGetValue(id, out source))
            {
                using (var stream = EmbeddedResourceReader.LoadResourceStream($"Errata.Tests/Data/{id}"))
                using (var reader = new StreamReader(stream))
                {
                    source = new Source(id, reader.ReadToEnd().Replace("\r\n", "\n"));
                    _lookup[id] = source;
                }
            }

            return true;
        }
    }
}
