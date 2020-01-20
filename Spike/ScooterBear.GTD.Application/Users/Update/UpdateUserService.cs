using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.Update
{
    public class UpdateUserService : IServiceAsyncOptionalAlternativeOutcome<UpdateUserServiceArgs,
        UpdateUserServiceResult, ExistingUser.EnableUserOutcome>
    {
        public Task<Option<UpdateUserServiceResult, ExistingUser.EnableUserOutcome>> Run(UpdateUserServiceArgs arg)
        {
            throw new System.NotImplementedException();
        }
    }
}
