using Autofac;

namespace ScooterBear.GTD.Fakes
{
    public class FakesAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(FakesAutofacModule).Assembly).AsImplementedInterfaces();
        }
    }
}
