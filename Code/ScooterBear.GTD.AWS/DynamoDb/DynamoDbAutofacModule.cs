using Autofac;

namespace ScooterBear.GTD.AWS.DynamoDb
{
    public class DynamoDbAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(DynamoDbAutofacModule).Assembly).AsImplementedInterfaces();
        }
    }
}
