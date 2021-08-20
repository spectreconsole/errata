using System.Diagnostics.CodeAnalysis;

namespace Errata
{
    public interface ISourceRepository
    {
        bool TryGet(string id, [NotNullWhen(true)] out Source? source);
    }
}
