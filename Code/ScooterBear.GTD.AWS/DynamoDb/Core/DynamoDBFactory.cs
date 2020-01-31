using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace ScooterBear.GTD.AWS.DynamoDb.Core
{
    public interface IDynamoDBFactory
    {
        DynamoDb Create();
    }
    
    public class DynamoDBFactory : IDynamoDBFactory
    {
        public DynamoDb Create()
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            DynamoDBContext context = new DynamoDBContext(client);
            return new DynamoDb(client, context);
        }
    }

    public class DynamoDb : IDisposable
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;
        //private readonly ILogger<DynamoDb> _logger;

        public DynamoDb(AmazonDynamoDBClient client, DynamoDBContext context)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //_logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SaveAsync<T>(T value, CancellationToken cancellationToken = default(CancellationToken))
            where T : IDynamoDbTable
        {
            try
            {
                //TODO:  Come back we should have an exponential back off and retry
                //TODO:  Fall and surface back to the user!!!
                //RESEARCH:  Does the .Net Framework use Polly or do I have to implement it?
                await _context.SaveAsync(value, cancellationToken);
            }
            catch (Exception ex)
            {
                //TODO:  Right this wrong!!!!
                //_logger.LogError(new EventId(301), ex, "Error saving to Dynamo.");
                throw;
            };
        }

        public async Task<T> LoadAsync<T>(
            object hashKey,
            object rangeKey,
            CancellationToken cancellationToken = default(CancellationToken))
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
                //TODO:  Right this wrong!!!!
                //_logger.LogError(new EventId(301), ex, "Error saving to Dynamo.");
                throw;
            };
        }

        public void Dispose()
        {
            _context.Dispose();
            _client.Dispose();
        }
    }
}
