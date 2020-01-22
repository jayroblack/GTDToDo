using System;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.DynamoDb.Users
{
    public class
        PersistUpdatedUserService : IServiceAsyncOptionalAlternativeOutcome<PersistUpdatedUserServiceArgs, PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>
    {
        public Task<Option<PersistUpdatedUserServiceResult, PersistUpdatedUserOutcome>> Run(PersistUpdatedUserServiceArgs arg)
        {
            //TODO:  Map values to the table and then save.
            //TODO:  Figure out what exception is thrown when there is a version conflict. 
            throw new NotImplementedException();
        }
    }
}
