using System.Reflection;

namespace Securely.Infrastructure;
public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
