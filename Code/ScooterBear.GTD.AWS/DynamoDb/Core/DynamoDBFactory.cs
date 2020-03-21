using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Logging;

namespace ScooterBear.GTD.AWS.DynamoDb.Core
{
    public interface IDynamoDBFactory
    {
        DynamoDb Create();
    }

    public class DynamoDBFactory : IDynamoDBFactory
    {
        private readonly ILogger<DynamoDb> _logger;

        public DynamoDBFactory(ILogger<DynamoDb> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public DynamoDb Create()
        {
            var client = new AmazonDynamoDBClient();
            var context = new DynamoDBContext(client);
            return new DynamoDb(client, context, _logger);
        }
    }
}