﻿using System;
using Autofac;
using FluentAssertions;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.IntegrationTests.UserProject
{
    [Collection("DynamoDbDockerTests")]
    public class AsThePersistUpdateProjectServiceI
    {
        public AsThePersistUpdateProjectServiceI(RunDynamoDbDockerFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        private readonly RunDynamoDbDockerFixture _fixture;

        [Fact]
        public async void ShouldReturnConflictIfStaleRead()
        {
            var userId = Guid.NewGuid().ToString();
            _fixture.ProfileFactory.SetUserProfile(new Profile(userId));

            var createUserProject = _fixture.Container
                .Resolve<IServiceOpt<CreateNewProjectArg, CreateNewProjectResult,
                    CreateProjectOutcomes>>();

            var projectId = Guid.NewGuid().ToString();
            var optionResult = await
                createUserProject.Run(new CreateNewProjectArg(projectId, "Project"));

            var persistService = _fixture.Container
                .Resolve<IServiceOpt<PersistUpdateProjectArg, PersistUpdateProjectResult,
                    PersistUpdateProjectOutcome>>();

            optionResult.Match(async some =>
            {
                var project = some.Project;
                var fudgedVersionProject = new Project(project.Id, project.UserId, project.Name, project.Count,
                    project.CountOverDue, DateTime.UtcNow, 100, project.IsDeleted);

                var persistOptionResult =
                    await persistService.Run(new PersistUpdateProjectArg(fudgedVersionProject));

                persistOptionResult.Match(some => Assert.False(true, "Should Fail to persist."),
                    outcome => outcome.Should().Be(PersistUpdateProjectOutcome.Conflict));
            }, outcomes => Assert.True(false, "Failed to Create Project."));
        }
    }
}