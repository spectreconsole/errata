using System;
using System.IO;
using System.Reflection;

namespace Errata.Tests
{
    public static class EmbeddedResourceReader
    {
        public static Stream LoadResourceStream(string resourceName)
        {
            if (resourceName is null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            var assembly = typeof(EmbeddedResourceRepository).Assembly;
            resourceName = resourceName.Replace("/", ".");

            return assembly.GetManifestResourceStream(resourceName);
        }

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
}
