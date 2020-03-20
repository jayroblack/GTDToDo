using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
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

    public class DynamoDb : IDisposable
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;
        private readonly ILogger<DynamoDb> _logger;

        public DynamoDb(AmazonDynamoDBClient client,
            DynamoDBContext context, ILogger<DynamoDb> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Dispose()
        {
            _context.Dispose();
            _client.Dispose();
        }

        public async Task SaveAsync<T>(T value, bool consistentRead = false,
            CancellationToken cancellationToken = default)
            where T : IDynamoDbTable
        {
            try
            {
                //TODO:  Come back we should have an exponential back off and retry
                //TODO:  Fall and surface back to the user!!!
                //RESEARCH:  Does the .Net Framework use Polly or do I have to implement it?
                await _context.SaveAsync(value, new DynamoDBOperationConfig {ConsistentRead = consistentRead},
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(301), ex, "Error saving to Dynamo.");
                throw;
            }

            ;
        }

        public async Task<T> LoadAsync<T>(
            object hashKey,
            object rangeKey,
            CancellationToken cancellationToken = default)
            where T : IDynamoDbTable
        {
            try
            {
                //TODO:  Come back we should have an exponential back off and retry
                //TODO:  Fall and surface back to the user!!!
                //RESEARCH:  Does the .Net Framework use Polly or do I have to implement it?
                return await _context.LoadAsync<T>(hashKey, rangeKey, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(301), ex, "Error saving to Dynamo.");
                throw;
            }

            ;
        }

        public AsyncSearch<T> QueryAsync<T>(
            object hashKey, QueryOperator op, IEnumerable<object> values,
            DynamoDBOperationConfig operationConfig = null)
        {
            return _context.QueryAsync<T>(hashKey, op, values, operationConfig);
        }

        public AsyncSearch<T> QueryAsync<T>(
            object hashKey, DynamoDBOperationConfig operationConfig = null)
        {
            return _context.QueryAsync<T>(hashKey, operationConfig);
        }
    }
}