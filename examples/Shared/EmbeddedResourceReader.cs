using System;
using System.IO;
using System.Reflection;

namespace Example;

public static class EmbeddedResourceReader
{
    public static Stream LoadResourceStream(Assembly assembly, string resourceName)
    {
        if (assembly is null)
        {
            throw new ArgumentNullException(nameof(assembly));
        }

        if (resourceName is null)
        {
            throw new ArgumentNullException(nameof(resourceName));
        }

        resourceName = resourceName.Replace("/", ".");
        return assembly.GetManifestResourceStream(resourceName);
    }
}
