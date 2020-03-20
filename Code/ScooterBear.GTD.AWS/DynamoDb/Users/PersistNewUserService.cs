using System;
using System.Threading;
using System.Threading.Tasks;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AWS.DynamoDb.Users
{
    public class PersistNewUserService : IService<PersistNewUserArgs, PersistNewUserResult>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;
        private readonly IMapFrom<UserProjectLabelDynamoDbTable, NewUser> _mapFrom;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> _mapTo;

        public PersistNewUserService(IDynamoDBFactory dynamoDbFactory,
            IMapFrom<UserProjectLabelDynamoDbTable, NewUser> mapFrom,
            IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> mapTo)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
            _mapFrom = mapFrom ?? throw new ArgumentNullException(nameof(mapFrom));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<PersistNewUserResult> Run(PersistNewUserArgs arg)
        {
            var table = _mapFrom.MapFrom(arg.NewUser);

            using (var dynamoDb = _dynamoDbFactory.Create())
            {
                await dynamoDb.SaveAsync(table, arg.ConsistentRead);
                var userRetrieved =
                    await dynamoDb.LoadAsync<UserProjectLabelDynamoDbTable>(table.ID, UserProjectLabelTableData.User,
                        CancellationToken.None);

                var readonlyUser = _mapTo.MapTo(userRetrieved);
                return new PersistNewUserResult(readonlyUser);
            }
        }
    }
}