using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using ScooterBear.GTD.DynamoDb.Dynamo;

namespace ScooterBear.GTD.IntegrationTests
{
    public class DynamoDBLoccalFactory : IDynamoDBFactory
    {
        public DynamoDb.Dynamo.DynamoDb Create()
        {
            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
            clientConfig.ServiceURL = "http://localhost:8000";
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(clientConfig);
            DynamoDBContext context = new DynamoDBContext(client);
            return new DynamoDb.Dynamo.DynamoDb(client, context);
        }
    }
}
