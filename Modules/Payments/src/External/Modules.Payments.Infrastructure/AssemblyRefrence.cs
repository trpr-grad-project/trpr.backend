using System.Reflection;

namespace Modules.Payments.Infrastructure;

public static class AssemblyRefrence
{
    public static Assembly Assembly => typeof(AssemblyRefrence).Assembly;
}
