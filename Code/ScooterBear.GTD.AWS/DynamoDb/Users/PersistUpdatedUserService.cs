using System;
using System.Threading;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AWS.DynamoDb.Users
{
    public class PersistUpdatedUserService : IServiceOptOutcomes<PersistUpdatedUserServiceArgs,
        PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>
    {
        private readonly IDynamoDBFactory _dynamoDbFactory;
        private readonly IMapFrom<UserProjectLabelDynamoDbTable, User> _mapper;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> _mapTo;

        public PersistUpdatedUserService(IDynamoDBFactory dynamoDbFactory,
            IMapFrom<UserProjectLabelDynamoDbTable, User> mapper,
            IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> mapTo)
        {
            _dynamoDbFactory = dynamoDbFactory ?? throw new ArgumentNullException(nameof(dynamoDbFactory));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }

        public async Task<Option<PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>> Run(
            PersistUpdatedUserServiceArgs arg)
        {
            var table = _mapper.MapFrom(arg.User);
            using (var _dynamoDb = _dynamoDbFactory.Create())
            {
                try
                {
                    await _dynamoDb.SaveAsync(table);

                    UserProjectLabelDynamoDbTable userRetrieved =
                        await _dynamoDb.LoadAsync<UserProjectLabelDynamoDbTable>(table.ID, UserProjectLabelTableData.User,
                            CancellationToken.None);

                    var readonlyUser = _mapTo.MapTo(userRetrieved);
                    return Option.Some<PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>(
                        new PersistUpdatedUserServiceResult(readonlyUser));
                }
                catch (Exception ex)
                {
                    //TODO:  Figure out what exception is thrown when there is a version conflict. 
                    return Option.None<PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>(
                        PersistUpdatedUserOutcome.Conflict);
                }
            }
        }
    }
}
