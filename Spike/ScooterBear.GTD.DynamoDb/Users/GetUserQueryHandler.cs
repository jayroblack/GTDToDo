using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Optional;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.DynamoDb.Dynamo;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.DynamoDb.Users
{
    public class GetUserQueryHandler : IQueryHandlerAsync<GetUserQueryArgs, GetUserQueryResult>
    {
        public async Task<Option<GetUserQueryResult>> Run(GetUserQueryArgs query)
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            DynamoDBContext context = new DynamoDBContext(client);
            var result =
                await context.LoadAsync<UserProjectLabelDynamoDbTable>(query.UserId, UserProjectLabelTableData.User);
            return Option.Some(new GetUserQueryResult(((INewuser) result)));
        }
    }
}
