using System;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.Update
{
    public class UpdateUserService : IServiceOptOutcomes<UpdateUserServiceArgs,
        UpdateUserServiceResult, UpdateUserService.UpdateUserOutcome>
    {
        private readonly IQueryHandler<GetUserQueryArgs, GetUserQueryResult> _getUser;

        private readonly
            IServiceOptOutcomes<PersistUpdatedUserServiceArgs, PersistUpdatedUserServiceResult,
                PersistUpdatedUserOutcome> _persistUpdatedUser;

        public enum UpdateUserOutcome
        {
            DoesNotExist,
            VersionConflict,
            UnprocessableEntity
        }

        public UpdateUserService(IQueryHandler<GetUserQueryArgs, GetUserQueryResult> getUser,
            IServiceOptOutcomes<PersistUpdatedUserServiceArgs, PersistUpdatedUserServiceResult,
                PersistUpdatedUserOutcome> persistUpdatedUser)
        {
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
            _persistUpdatedUser = persistUpdatedUser ?? throw new ArgumentNullException(nameof(persistUpdatedUser));
        }

        public async Task<Option<UpdateUserServiceResult, UpdateUserOutcome>> Run(UpdateUserServiceArgs arg)
        {
            var userExistsOption = await _getUser.Run(new GetUserQueryArgs(arg.ID));
            if (!userExistsOption.HasValue)
                return Option.None<UpdateUserServiceResult, UpdateUserOutcome>(UpdateUserOutcome
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

                if (arg.IsEmailVerified.HasValue && arg.IsEmailVerified.GetValueOrDefault())
                    user.VerifyEmail();
            }
            catch (ArgumentException e)
            {
                return Option.None<UpdateUserServiceResult, UpdateUserOutcome>(UpdateUserOutcome
                    .UnprocessableEntity);
            }

            var updatedExistUserOption = await _persistUpdatedUser.Run(new PersistUpdatedUserServiceArgs(user));

            return updatedExistUserOption.Match(
                some => Option.Some<UpdateUserServiceResult, UpdateUserOutcome>(
                        new UpdateUserServiceResult(some.UpdatedUser)),
                none => Option.None<UpdateUserServiceResult, UpdateUserOutcome>(UpdateUserOutcome
                    .VersionConflict));
        }
    }
}
