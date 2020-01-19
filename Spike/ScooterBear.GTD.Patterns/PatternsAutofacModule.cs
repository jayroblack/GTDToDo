using Autofac;

namespace ScooterBear.GTD.Patterns
{
    public class PatternsAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(PatternsAutofacModule).Assembly).AsImplementedInterfaces();
        }
    }
}
