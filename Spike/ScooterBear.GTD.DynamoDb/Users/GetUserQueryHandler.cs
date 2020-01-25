using System;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.DynamoDb.Dynamo;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.DynamoDb.Users
{
    public class GetUserQueryHandler : IQueryHandler<GetUserQueryArgs, GetUserQueryResult>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> _mapTo;

        public GetUserQueryHandler(IDynamoDBFactory dynamoDbFactory,
            IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> mapTo)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<Option<GetUserQueryResult>> Run(GetUserQueryArgs query)
        {
            using (var _dynamoDb = _dynamoDbFactory.Create())
            {
                var result =
                    await _dynamoDb.LoadAsync<UserProjectLabelDynamoDbTable>(query.UserId,
                        UserProjectLabelTableData.User);

                var user = _mapTo.MapTo(result);
                return Option.Some(new GetUserQueryResult(user));
            }
        }
    }
}
