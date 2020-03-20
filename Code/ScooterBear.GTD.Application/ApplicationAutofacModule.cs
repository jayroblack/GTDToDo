using Autofac;
using ScooterBear.GTD.Patterns.Domain;

namespace ScooterBear.GTD.Application
{
    public class ApplicationAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ApplicationAutofacModule).Assembly).AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(DomainEventHandlerStrategyAsync<>))
                .As(typeof(IDomainEventHandlerStrategyAsync<>))
                .InstancePerLifetimeScope();
        }
    }
}