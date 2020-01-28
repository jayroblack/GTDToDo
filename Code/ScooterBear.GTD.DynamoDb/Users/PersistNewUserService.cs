using System;
using System.Threading;
using System.Threading.Tasks;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.DynamoDb.Dynamo;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.DynamoDb.Users
{
    public class PersistNewUserService : IService<PersistNewUserServiceArgs, PersistNewUserServiceResult>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;
        private readonly IMapFrom<UserProjectLabelDynamoDbTable, NewUser> _mapFrom;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> _mapTo;
        //private readonly IDomainEventHandlerStrategyAsync<NewUserCreatedEvent> _newUserCreated;

        public PersistNewUserService(IDynamoDBFactory dynamoDbFactory,
            IMapFrom<UserProjectLabelDynamoDbTable, NewUser> mapFrom,
            IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> mapTo
            /*IDomainEventHandlerStrategyAsync<NewUserCreatedEvent> newUserCreated*/)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
            _mapFrom = mapFrom ?? throw new ArgumentNullException(nameof(mapFrom));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
            //_newUserCreated = newUserCreated ?? throw new ArgumentNullException(nameof(newUserCreated));
        }

        public async Task<PersistNewUserServiceResult> Run(PersistNewUserServiceArgs arg)
        {
            var table = _mapFrom.MapFrom(arg.NewUser);

            using (var _dynamoDb = _dynamoDbFactory.Create())
            {
                await _dynamoDb.SaveAsync(table, CancellationToken.None);
                var userRetrieved =
                    await _dynamoDb.LoadAsync<UserProjectLabelDynamoDbTable>(table.ID, UserProjectLabelTableData.User, CancellationToken.None);

                var readonlyUser = _mapTo.MapTo(userRetrieved);
                //await _newUserCreated.HandleEventsAsync(new NewUserCreatedEvent(readonlyUser), CancellationToken.None);
                return new PersistNewUserServiceResult(readonlyUser);
            }
        }
    }
}
