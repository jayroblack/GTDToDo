using System;
using System.Threading;
using System.Threading.Tasks;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;
using ScooterBear.GTD.Patterns.Domain;

namespace ScooterBear.GTD.Application.Users.New
{
    public class WhenNewUserCreatedBootstrapUser : IDomainEventHandlerAsync<NewUserCreatedEvent>
    {
        private readonly ICreateIdsStrategy _createIdsStrategy;
        private readonly IServiceOptOutcomes<CreateNewUserProjectServiceArg, CreateNewUserProjectServiceResult, CreateUserProjectOutcomes> _createInboxProject;

        public WhenNewUserCreatedBootstrapUser(
            ICreateIdsStrategy createIdsStrategy,
            IServiceOptOutcomes<CreateNewUserProjectServiceArg, CreateNewUserProjectServiceResult, CreateUserProjectOutcomes> createInboxProject)
        {
            _createIdsStrategy = createIdsStrategy ?? throw new ArgumentNullException(nameof(createIdsStrategy));
            _createInboxProject = createInboxProject ?? throw new ArgumentNullException(nameof(createInboxProject));
        }
        public async Task HandleAsync(NewUserCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            var createNewProjectService = new CreateNewUserProjectServiceArg(_createIdsStrategy.NewId(), "Inbox", true);
            await _createInboxProject.Run(createNewProjectService);
        }
    }
}
