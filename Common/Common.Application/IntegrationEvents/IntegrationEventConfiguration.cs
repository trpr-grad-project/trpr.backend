using System.Reflection;
using Common.Application.DomainEvents;

namespace Common.Application.IntegrationEvents
{

    public class IntegrationEventConfiguration
    {
        public DecoratorConfiguration Decorators { get; set; } = new();
        public AssemblyConfiguration Assemblies { get; set; } = new();

        public IntegrationEventConfiguration AddPipeline(Type openGenericDecorator)
        {
            Decorators = Decorators.AddDecorator(openGenericDecorator);
            return this;
        }

        public IntegrationEventConfiguration AddAssembly(Assembly assembly)
        {
            Assemblies = Assemblies.AddAssembly(assembly);
            return this;
        }

        public IntegrationEventConfiguration AddAssemblies(params Assembly[] assemblies)
        {
            Assemblies = Assemblies.AddAssemblies(assemblies);
            return this;
        }
    }

}