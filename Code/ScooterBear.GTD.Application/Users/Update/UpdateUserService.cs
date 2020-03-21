using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.Users.Update
{
    public enum UpdateUserOutcome
    {
        DoesNotExist,
        VersionConflict,
        UnprocessableEntity,
        NotAuthorized
    }

    public class UpdateUserService : IServiceOpt<UpdateUserArg,
        UpdateUserResult, UpdateUserOutcome>
    {
        private readonly IQueryHandler<GetUserArg, GetUserQueryResult> _getUser;
        private readonly ILogger _logger;

        private readonly
            IServiceOpt<PersistUpdatedUserServiceArg, PersistUpdatedUserServiceResult,
                PersistUpdatedUserOutcome> _persistUpdatedUser;

        private readonly IProfileFactory _profileFactory;

        public UpdateUserService(
            IProfileFactory profileFactory,
            ILogger logger,
            IQueryHandler<GetUserArg, GetUserQueryResult> getUser,
            IServiceOpt<PersistUpdatedUserServiceArg, PersistUpdatedUserServiceResult,
                PersistUpdatedUserOutcome> persistUpdatedUser)
        {
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
            _persistUpdatedUser = persistUpdatedUser ?? throw new ArgumentNullException(nameof(persistUpdatedUser));
        }

        public async Task<Option<UpdateUserResult, UpdateUserOutcome>> Run(UpdateUserArg arg)
        {
            var userExistsOption = await _getUser.Run(new GetUserArg(arg.ID));
            if (!userExistsOption.HasValue)
                return Option.None<UpdateUserResult, UpdateUserOutcome>(UpdateUserOutcome
                    .DoesNotExist);

            User user = null;
            userExistsOption.MatchSome(some =>
            {
                var existingUser = some.User;
                user = new User(existingUser.ID, existingUser.FirstName, existingUser.LastName, existingUser.Email,
                    existingUser.BillingId, existingUser.AuthId,
                    existingUser.VersionNumber, existingUser.DateCreated);
            });

            var profile = _profileFactory.GetCurrentProfile();
            if (user.ID != profile.UserId)
                return Option.None<UpdateUserResult, UpdateUserOutcome>(UpdateUserOutcome
                    .NotAuthorized);

            try
            {
                user.SetFirstName(arg.FirstName);
                user.SetLastName(arg.LastName);
                user.SetEmail(arg.Email);
                user.SetBillingId(arg.BillingId);
                user.SetAuthId(arg.AuthId);
                user.SetVersionNumber(arg.VersionNumber);
            }
            catch (ArgumentException e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                return Option.None<UpdateUserResult, UpdateUserOutcome>(UpdateUserOutcome
                    .UnprocessableEntity);
            }

            var updatedExistUserOption = await _persistUpdatedUser.Run(new PersistUpdatedUserServiceArg(user));

            return updatedExistUserOption.Match(
                some => Option.Some<UpdateUserResult, UpdateUserOutcome>(
                    new UpdateUserResult(some.UpdatedUser)),
                none => Option.None<UpdateUserResult, UpdateUserOutcome>(UpdateUserOutcome
                    .VersionConflict));
        }
    }
}