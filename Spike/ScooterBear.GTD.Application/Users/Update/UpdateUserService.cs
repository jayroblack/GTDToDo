using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.Update
{
    public class UpdateUserService : IServiceAsyncOptionalAlternativeOutcome<UpdateUserServiceArgs,
        UpdateUserServiceResult, ExistingUser.ModifyUserOutcome>
    {
        public Task<Option<UpdateUserServiceResult, ExistingUser.ModifyUserOutcome>> Run(UpdateUserServiceArgs arg)
        {
            throw new System.NotImplementedException();
        }
    }
}
