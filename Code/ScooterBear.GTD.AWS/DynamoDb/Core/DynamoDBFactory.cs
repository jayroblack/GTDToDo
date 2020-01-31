using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

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

        public DynamoDb(AmazonDynamoDBClient client, DynamoDBContext context)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task SaveAsync<T>(T value, CancellationToken cancellationToken = default(CancellationToken))
            where T : IDynamoDbTable
        {
            return _context.SaveAsync(value, cancellationToken);
        }

        public Task<T> LoadAsync<T>(
            object hashKey,
            object rangeKey,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : IDynamoDbTable
        {
            return _context.LoadAsync<T>(hashKey, rangeKey, cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
            _client.Dispose();
        }
    }
}
