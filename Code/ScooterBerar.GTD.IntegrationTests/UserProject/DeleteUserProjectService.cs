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
        public AsTheDeleteUserProjectServiceI(RunDynamoDbDockerFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly RunDynamoDbDockerFixture _fixture;

        [Fact]
        public async void ShouldDeleteProject()
        {
            var userId = Guid.NewGuid().ToString();
            _fixture.ProfileFactory.SetUserProfile(new Profile(userId));
            var createUserProject = _fixture.Container
                .Resolve<IServiceOptOutcomes<CreateNewProjectArg, CreateNewProjectResult,
                    CreateUserProjectOutcomes>>();

            var queryProject = _fixture.Container.Resolve<IQueryHandler<GetProject, GetProjectResult>>();

            var userProjects =
                _fixture.Container.Resolve<IQueryHandler<GetProjects, GetProjectsResult>>();

            var projectId = Guid.NewGuid().ToString();
            var optionResult = await
                createUserProject.Run(new CreateNewProjectArg(projectId, "IWillBeDeleted"));

            optionResult.HasValue.Should().BeTrue("Failed To Create Project");

            var queryProjBeforeDelete = await queryProject.Run(new GetProject(projectId));

            queryProjBeforeDelete.HasValue.Should().BeTrue("Not Deleted Yet");

            var deleteProjectService = _fixture.Container
                .Resolve<IServiceOptOutcomes<DeleteProjectArgs, DeleteProjectResult,
                    DeleteUserProjectOutcome>>();

            var deleteOption = await deleteProjectService.Run(new DeleteProjectArgs(projectId));

            deleteOption.HasValue.Should().BeTrue("Delete Should Succeed");

            var queryProjAfterDelete = await queryProject.Run(new GetProject(projectId));

            queryProjAfterDelete.Match(some =>
                {
                    some.UserProject.Should().NotBeNull();
                    some.UserProject.IsDeleted.Should().BeTrue();
                },
                () => Assert.False(true, "Should Return a value that is deleted"));

            var projectsOption = await userProjects.Run(new GetProjects(userId));
            projectsOption.HasValue.Should().BeFalse("When returning lists delete is omitted.");
        }
    }
}