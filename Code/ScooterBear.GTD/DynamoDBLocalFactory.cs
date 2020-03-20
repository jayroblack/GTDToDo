using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Logging;
using ScooterBear.GTD.AWS.DynamoDb.Core;

namespace ScooterBear.GTD
{
    public class DynamoDBLocalFactory : IDynamoDBFactory
    {
        private readonly ILogger<DynamoDb> _logger;

        public DynamoDBLocalFactory(ILogger<DynamoDb> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public DynamoDb Create()
        {
            var clientConfig = new AmazonDynamoDBConfig();
            clientConfig.ServiceURL = "http://localhost:8000";
            var client = new AmazonDynamoDBClient(clientConfig);
            var context = new DynamoDBContext(client);
            return new DynamoDb(client, context, _logger);
        }
    }
}