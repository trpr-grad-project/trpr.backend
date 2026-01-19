using System.Reflection;
namespace Modules.Users.Persistence;

public static class AssemblyRefrence
{
    public static Assembly Assembly => typeof(AssemblyRefrence).Assembly;
}
