using System.Runtime.CompilerServices;

namespace Errata.Tests;

public static class VerifyConfiguration
{
    [ModuleInitializer]
    public static void Init()
    {
        Verifier.DerivePathInfo(Expectations.Initialize);
    }
}
