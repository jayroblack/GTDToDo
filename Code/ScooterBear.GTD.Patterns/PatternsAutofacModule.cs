using Autofac;
using ScooterBear.GTD.Patterns.Domain;

namespace ScooterBear.GTD.Patterns
{
    public class PatternsAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(PatternsAutofacModule).Assembly).AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(DomainEventHandlerStrategyAsync<>))
                .As(typeof(IDomainEventHandlerStrategyAsync<>))
                .InstancePerLifetimeScope();
        }
    }
}