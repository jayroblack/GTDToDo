using Autofac;

namespace ScooterBear.GTD.DynamoDb
{
    public class DynamoDbAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(DynamoDbAutofacModule).Assembly).AsImplementedInterfaces();
        }
    }
}
