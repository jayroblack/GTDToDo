using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Optional;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.DynamoDb.Dynamo;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.DynamoDb.Users
{
    public class GetUserQueryHandler : IQueryHandlerAsync<GetUserQueryArgs, GetUserQueryResult>
    {
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> _mapTo;

        public GetUserQueryHandler(IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> mapTo)
        {
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<Option<GetUserQueryResult>> Run(GetUserQueryArgs query)
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            DynamoDBContext context = new DynamoDBContext(client);
            var result =
                await context.LoadAsync<UserProjectLabelDynamoDbTable>(query.UserId, UserProjectLabelTableData.User);

            var user = _mapTo.MapTo(result);
            return Option.Some(new GetUserQueryResult(user));
        }
    }
}
