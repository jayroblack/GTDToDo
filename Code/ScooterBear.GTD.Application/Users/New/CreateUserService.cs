using System;
using System.Threading;
using System.Threading.Tasks;
using Optional;
using Optional.Async.Extensions;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;
using ScooterBear.GTD.Patterns.Domain;

namespace ScooterBear.GTD.Application.Users.New
{
    public enum CreateUserServiceOutcome
    {
        UserExists
    }

    public class CreateUserService : IServiceOptOutcomes<CreateUserServiceArg,
        CreateUserServiceResult, CreateUserServiceOutcome>
    {
        private readonly IKnowTheDate _iKnowTheDate;
        private readonly IService<PersistNewUserServiceArgs, PersistNewUserServiceResult> _persistNewUserService;
        private readonly IQueryHandler<GetUserQueryArgs, GetUserQueryResult> _getUser;
        private readonly IDomainEventHandlerStrategyAsync<NewUserCreatedEvent> _newUserCreated;

        public CreateUserService(IKnowTheDate iKnowTheDate,
            ICreateIdsStrategy createIdsStrategy,
            IService<PersistNewUserServiceArgs, PersistNewUserServiceResult> persistNewUserService,
            IQueryHandler<GetUserQueryArgs, GetUserQueryResult> getUser,
            IDomainEventHandlerStrategyAsync<NewUserCreatedEvent> newUserCreated)
        {
            _iKnowTheDate = iKnowTheDate ?? throw new ArgumentNullException(nameof(iKnowTheDate));
            _persistNewUserService =
                persistNewUserService ?? throw new ArgumentNullException(nameof(persistNewUserService));
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
            _newUserCreated = newUserCreated ?? throw new ArgumentNullException(nameof(newUserCreated));
        }

        public async Task<Option<CreateUserServiceResult, CreateUserServiceOutcome>> Run(CreateUserServiceArg arg)
        {
            if (arg == null) throw new ArgumentNullException(nameof(arg));

            var queryOption = await _getUser.Run(new GetUserQueryArgs(arg.Id));

            return await 
                queryOption.MatchAsync(
                    async some => Option.None<CreateUserServiceResult, CreateUserServiceOutcome>(
                        CreateUserServiceOutcome.UserExists),
                    async () =>
                    {
                        var newUser = new NewUser(arg.Id, arg.FirstName, arg.LastName, arg.Email,
                            _iKnowTheDate.UtcNow());

                        var result = await
                            _persistNewUserService.Run(new PersistNewUserServiceArgs(newUser));

                        await _newUserCreated.HandleEventsAsync(new NewUserCreatedEvent(result.ReadonlyUser),
                            CancellationToken.None);

                        return Option.Some<CreateUserServiceResult, CreateUserServiceOutcome>(
                            new CreateUserServiceResult(result.ReadonlyUser));
                    });
        }
    }
}
