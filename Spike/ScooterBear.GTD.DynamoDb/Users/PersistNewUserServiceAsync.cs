using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.DynamoDb.Dynamo;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.DynamoDb.Users
{
    public class PersistNewUserServiceAsync : IServiceAsync<PersistNewUserServiceArgs, PersistNewUserServiceResult>
    {
        private readonly IMapFrom<UserProjectLabelDynamoDbTable, NewUser> _mapFrom;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> _mapTo;

        public PersistNewUserServiceAsync(IMapFrom<UserProjectLabelDynamoDbTable, NewUser> mapFrom,
            IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> mapTo)
        {
            _mapFrom = mapFrom ?? throw new ArgumentNullException(nameof(mapFrom));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<PersistNewUserServiceResult> Run(PersistNewUserServiceArgs arg)
        {
            var table = _mapFrom.MapFrom(arg.NewUser);

            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            DynamoDBContext context = new DynamoDBContext(client);
            await context.SaveAsync<UserProjectLabelDynamoDbTable>(table, CancellationToken.None);
            UserProjectLabelDynamoDbTable bookRetrieved =
                await context.LoadAsync<UserProjectLabelDynamoDbTable>(table.ID, table.DateCreated,
                    CancellationToken.None);

            var readonlyUser = _mapTo.MapTo(bookRetrieved);
            return new PersistNewUserServiceResult(readonlyUser);
        }
    }
}
