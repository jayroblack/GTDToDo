using System;
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
        private readonly RunDynamoDbDockerFixture _fixture;

        public AsThePersistUpdateProjectServiceI(RunDynamoDbDockerFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async void ShouldReturnConflictIfStaleRead()
        {
            var userId = Guid.NewGuid().ToString();
            _fixture.ProfileFactory.SetUserProfile(new Profile(userId));
            
            var createUserProject = _fixture.Container
                .Resolve<IServiceOptOutcomes<CreateNewUserProjectServiceArg, CreateNewUserProjectServiceResult,
                    CreateUserProjectOutcomes>>();

            var projectId = Guid.NewGuid().ToString();
            var optionResult = await
                createUserProject.Run(new CreateNewUserProjectServiceArg(projectId, userId, "Project"));

            var persistService = _fixture.Container
                .Resolve<IServiceOptOutcomes<PersistUpdateProjectServiceArgs, PersistUpdateProjectServiceResult,
                    PersistUpdateProjectOutcome>>();

            optionResult.Match(async some =>
            {
                var project = some.Project;
                var fudgedVersionProject = new Project(project.Id, project.UserId, project.Name, project.Count, project.IsDeleted, project.CountOverDue, 100, DateTime.UtcNow);

                var persistOptionResult =
                    await persistService.Run(new PersistUpdateProjectServiceArgs(fudgedVersionProject));

                persistOptionResult.Match(some => Assert.False(true, "Should Fail to persist."),
                    outcome => outcome.Should().Be(PersistUpdateProjectOutcome.Conflict));

            }, outcomes => Assert.True(false, "Failed to Create Project."));
        }
    }
}
