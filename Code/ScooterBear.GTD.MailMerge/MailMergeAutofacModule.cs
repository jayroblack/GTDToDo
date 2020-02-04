using Autofac;

namespace ScooterBear.GTD.MailMerge
{
    public class MailMergeAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(MailMergeAutofacModule).Assembly).AsImplementedInterfaces();
        }
    }
}
