using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Optional;
using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.DynamoDb.Dynamo;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.DynamoDb.Users
{
    public class
        PersistUpdatedUserService : IServiceAsyncOptionalOutcomes<PersistUpdatedUserServiceArgs, PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>
    {
        private readonly IMapFrom<UserProjectLabelDynamoDbTable, User> _mapper;
        private readonly IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> _mapTo;

        public PersistUpdatedUserService(IMapFrom<UserProjectLabelDynamoDbTable, User> mapper,
            IMapTo<UserProjectLabelDynamoDbTable, ReadonlyUser> mapTo)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mapTo = mapTo ?? throw new ArgumentNullException(nameof(mapTo));
        }
        public async Task<Option<PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>> Run(PersistUpdatedUserServiceArgs arg)
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            DynamoDBContext context = new DynamoDBContext(client);

            var table = _mapper.MapFrom(arg.User);
            await context.SaveAsync(table);

            try
            {
                UserProjectLabelDynamoDbTable userRetrieved =
                        await context.LoadAsync<UserProjectLabelDynamoDbTable>(table.ID, table.DateCreated,
                            CancellationToken.None);

                var readonlyUser = _mapTo.MapTo(userRetrieved);
                return Option.Some<PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>(
                    new PersistUpdatedUserServiceResult(readonlyUser));

            }
            catch (Exception ex)
            {
                //TODO:  Figure out what exception is thrown when there is a version conflict. 
                return Option.None<PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>(PersistUpdatedUserOutcome.Conflict);
            }
        }
    }
}
