using Autofac;

namespace ScooterBear.GTD
{
    public class GtdAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(GtdAutofacModule).Assembly).AsImplementedInterfaces();
        }
    }
}
