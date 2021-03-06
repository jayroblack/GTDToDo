﻿using System;
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

        private readonly IServiceOpt<CreateNewProjectArg, CreateNewProjectResult, CreateProjectOutcomes>
            _createInboxProject;

        public WhenNewUserCreatedBootstrapUser(
            ICreateIdsStrategy createIdsStrategy,
            IServiceOpt<CreateNewProjectArg, CreateNewProjectResult, CreateProjectOutcomes>
                createInboxProject)
        {
            _createIdsStrategy = createIdsStrategy ?? throw new ArgumentNullException(nameof(createIdsStrategy));
            _createInboxProject = createInboxProject ?? throw new ArgumentNullException(nameof(createInboxProject));
        }

        public async Task HandleAsync(NewUserCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            var createNewProjectService = new CreateNewProjectArg(_createIdsStrategy.NewId(), "Inbox", true);
            await _createInboxProject.Run(createNewProjectService);
        }
    }
}