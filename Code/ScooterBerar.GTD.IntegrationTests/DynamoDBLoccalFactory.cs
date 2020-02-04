using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Logging;
using ScooterBear.GTD.AWS.DynamoDb.Core;

namespace ScooterBear.GTD.IntegrationTests
{
    public class DynamoDBLoccalFactory : IDynamoDBFactory
    {
        private readonly ILogger<DynamoDb> _logger;

        public DynamoDBLoccalFactory(ILogger<DynamoDb> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public DynamoDb Create()
        {
            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
            clientConfig.ServiceURL = "http://localhost:8000";
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(clientConfig);
            DynamoDBContext context = new DynamoDBContext(client);
            return new DynamoDb(client, context, _logger);
        }
    }
}
