using System;
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

        public PersistUpdatedUserService(IMapFrom<UserProjectLabelDynamoDbTable, User> mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<Option<PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>> Run(PersistUpdatedUserServiceArgs arg)
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            DynamoDBContext context = new DynamoDBContext(client);

            var table = _mapper.MapFrom(arg.User);
            await context.SaveAsync(table);
            //TODO:  Figure out what exception is thrown when there is a version conflict. 
            throw new NotImplementedException();
        }
    }
}
