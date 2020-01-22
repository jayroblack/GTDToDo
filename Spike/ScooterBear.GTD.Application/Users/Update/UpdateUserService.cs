using System;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.Update
{
    public class UpdateUserService : IServiceAsyncOptionalAlternativeOutcome<UpdateUserServiceArgs,
        UpdateUserServiceResult, UpdateUserService.UpdateUserOutcome>
    {
        private readonly IQueryHandlerAsync<GetUserQueryArgs, GetUserQueryResult> _getUser;

        public enum UpdateUserOutcome
        {
            DoesNotExist,
            VersionConflict,
            Success,
            EmailIsNotVerified,
            AuthIdIsNotDefined,
            PaymentOverdue
        }

        public UpdateUserService(IQueryHandlerAsync<GetUserQueryArgs, GetUserQueryResult> getUser)
        {
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
        }

        public Task<Option<UpdateUserServiceResult, UpdateUserService.UpdateUserOutcome>> Run(UpdateUserServiceArgs arg)
        {
            var desiredUserState = arg as IUser;
            throw new System.NotImplementedException();
        }
    }
}
