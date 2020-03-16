using System;
using Autofac;
using FluentAssertions;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.IntegrationTests.UserProject
{
    [Collection("DynamoDbDockerTests")]
    public class AsTheDeleteUserProjectServiceI
    {
        private readonly RunDynamoDbDockerFixture _fixture;

        public AsTheDeleteUserProjectServiceI(RunDynamoDbDockerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void ShouldDeleteProject()
        {
            var userId = Guid.NewGuid().ToString();
            _fixture.ProfileFactory.SetUserProfile(new Profile(userId));
            var createUserProject = _fixture.Container
                .Resolve<IServiceOptOutcomes<CreateNewUserProjectServiceArg, CreateNewUserProjectServiceResult,
                    CreateUserProjectOutcomes>>();

            var queryProject = _fixture.Container.Resolve<IQueryHandler<ProjectQuery, ProjectQueryResult>>();

            var userProjects =
                _fixture.Container.Resolve<IQueryHandler<GetUserProjectsQuery, GetUserProjectsQueryResult>>();

            var projectId = Guid.NewGuid().ToString();
            var optionResult = await
                createUserProject.Run(new CreateNewUserProjectServiceArg(projectId, userId, "IWillBeDeleted"));

            optionResult.HasValue.Should().BeTrue("Failed To Create Project");

            var queryProjBeforeDelete = await queryProject.Run(new ProjectQuery(projectId));

            queryProjBeforeDelete.HasValue.Should().BeTrue("Not Deleted Yet");

            var deleteProjectService = _fixture.Container
                .Resolve<IServiceOptOutcomes<DeleteUserProjectServiceArgs, DeleteUserProjectServiceResult,
                    DeleteUserProjectOutcome>>();

            var deleteOption = await deleteProjectService.Run(new DeleteUserProjectServiceArgs(projectId));

            deleteOption.HasValue.Should().BeTrue("Delete Should Succeed");

            var queryProjAfterDelete = await queryProject.Run(new ProjectQuery(projectId));

            queryProjAfterDelete.Match(some =>
                {
                    some.UserProject.Should().NotBeNull();
                    some.UserProject.IsDeleted.Should().BeTrue();
                },
                () => Assert.False(true, "Should Return a value that is deleted"));

            var projectsOption = await userProjects.Run(new GetUserProjectsQuery(userId));
            projectsOption.HasValue.Should().BeFalse("When returning lists delete is omitted.");
        }
    }
}
