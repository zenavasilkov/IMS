using System.Reflection;

namespace Infrastructure;

public static class AssemblyReference
{
    public readonly static Assembly Assembly = typeof(AssemblyReference).Assembly;
}
