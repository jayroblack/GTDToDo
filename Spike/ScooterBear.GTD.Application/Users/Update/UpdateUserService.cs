using System;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.Update
{
    public class UpdateUserService : IServiceAsyncOptionalOutcomes<UpdateUserServiceArgs,
        UpdateUserServiceResult, UpdateUserService.UpdateUserOutcome>
    {
        private readonly IQueryHandlerAsync<GetUserQueryArgs, GetUserQueryResult> _getUser;

        private readonly
            IServiceAsyncOptionalOutcomes<PersistUpdatedUserServiceArgs, PersistUpdatedUserServiceResult,
                PersistUpdatedUserOutcome> _persistUpdatedUser;

        public enum UpdateUserOutcome
        {
            DoesNotExist,
            VersionConflict,
            UnprocessableEntity
        }

        public UpdateUserService(IQueryHandlerAsync<GetUserQueryArgs, GetUserQueryResult> getUser,
            IServiceAsyncOptionalOutcomes<PersistUpdatedUserServiceArgs, PersistUpdatedUserServiceResult,
                PersistUpdatedUserOutcome> persistUpdatedUser)
        {
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
            _persistUpdatedUser = persistUpdatedUser ?? throw new ArgumentNullException(nameof(persistUpdatedUser));
        }

        public async Task<Option<UpdateUserServiceResult, UpdateUserOutcome>> Run(UpdateUserServiceArgs arg)
        {
            var userExistsOption = await _getUser.Run(new GetUserQueryArgs(arg.ID));
            if (!userExistsOption.HasValue)
                Option.None<UpdateUserServiceResult, UpdateUserOutcome>(UpdateUserOutcome
                    .DoesNotExist);

            User user = null;
            userExistsOption.MatchSome(some =>
            {
                var existingUser = some.User;
                user = new User(existingUser.ID, existingUser.FirstName, existingUser.LastName, existingUser.Email,
                    existingUser.IsEmailVerified, existingUser.BillingId, existingUser.AuthId,
                    existingUser.VersionNumber, existingUser.DateCreated);
            });

            try
            {
                user.SetFirstName(arg.FirstName);
                user.SetLastName(arg.LastName);
                user.SetEmail(arg.Email);
                user.SetBillingId(arg.BillingId);
                user.SetAuthId(arg.AuthId);

                if (arg.IsEmailVerified.GetValueOrDefault() && !user.IsEmailVerified.GetValueOrDefault())
                    user.VerifyEmail();

                if (!arg.IsAccountEnabled.GetValueOrDefault() && user.IsAccountEnabled.GetValueOrDefault())
                    user.DisableAccount();

                if (arg.IsAccountEnabled.GetValueOrDefault() &&
                    (!user.IsAccountEnabled.HasValue || user.IsAccountEnabled.GetValueOrDefault()))
                    user.EnableAccount();

            }
            catch (ArgumentException e)
            {
                return Option.None<UpdateUserServiceResult, UpdateUserOutcome>(UpdateUserOutcome
                    .UnprocessableEntity);
            }

            var updatedExistUserOption = await _persistUpdatedUser.Run(new PersistUpdatedUserServiceArgs(user));

            if (!updatedExistUserOption.HasValue)
                return Option.None<UpdateUserServiceResult, UpdateUserOutcome>(UpdateUserOutcome
                    .VersionConflict);

            return updatedExistUserOption.Match<Option<UpdateUserServiceResult, UpdateUserOutcome>>(
                some => Option.Some<UpdateUserServiceResult, UpdateUserOutcome>(
                        new UpdateUserServiceResult(some.UpdatedUser)),
                none => Option.None<UpdateUserServiceResult, UpdateUserOutcome>(UpdateUserOutcome
                    .VersionConflict));
        }
    }
}
