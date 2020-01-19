using System;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Abstractions.Users;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.DynamoDb.Users
{
    public class GetUserQueryHandler : IQueryHandlerAsync<GetUserQueryArgs, GetUserQueryResult>
    {
        public Task<Option<GetUserQueryResult>> Run(GetUserQueryArgs query)
        {
            throw new NotImplementedException();
        }
    }
}
