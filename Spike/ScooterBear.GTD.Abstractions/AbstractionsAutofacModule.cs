using Autofac;

namespace ScooterBear.GTD.Abstractions
{
    public class AbstractionsAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(AbstractionsAutofacModule).Assembly).AsImplementedInterfaces();
        }
    }
}
