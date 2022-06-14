using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Errata;

namespace Example;

public sealed class EmbeddedResourceRepository : ISourceRepository
{
    private readonly Dictionary<string, Source> _lookup;
    private readonly Assembly _assembly;

    public EmbeddedResourceRepository(Assembly assembly)
    {
        _lookup = new Dictionary<string, Source>(StringComparer.OrdinalIgnoreCase);
        _assembly = assembly;
    }

    public bool TryGet(string id, [NotNullWhen(true)] out Source source)
    {
        if (!_lookup.TryGetValue(id, out source))
        {
            using (var stream = EmbeddedResourceReader.LoadResourceStream(_assembly, id))
            using (var reader = new StreamReader(stream))
            {
                source = new Source(id, reader.ReadToEnd().Replace("\r\n", "\n"));
                _lookup[id] = source;
            }
        }

        return true;
    }
}
