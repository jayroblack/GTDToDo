using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using FluentAssertions;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.IntegrationTests.UserProject
{
    [Collection("DynamoDbDockerTests")]
    public class AsTheCreateNewUserProjectServiceI
    {
        public AsTheCreateNewUserProjectServiceI(RunDynamoDbDockerFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        private readonly RunDynamoDbDockerFixture _fixture;

        [Fact]
        public async void ShouldNotAllowANewProjectToBeCreatedWithAnExistingId()
        {
            var userId = Guid.NewGuid().ToString();
            var createUserProject = _fixture.Container
                .Resolve<IServiceOpt<CreateNewProjectArg, CreateNewProjectResult,
                    CreateUserProjectOutcomes>>();

            var id = Guid.NewGuid().ToString();
            var firstItem = new ProjectItem(id, "Project 1");
            var secondItem = new ProjectItem(id, "Project 2");

            var firstCreateOption = await
                createUserProject.Run(new CreateNewProjectArg(firstItem.Id, firstItem.Name));

            firstCreateOption.HasValue.Should().BeTrue();

            var secondCreateOption =
                await createUserProject.Run(new CreateNewProjectArg(secondItem.Id, secondItem.Name));

            secondCreateOption.Match(some => Assert.False(true, "Should Fail."),
                outcomes => outcomes.Should().Be(CreateUserProjectOutcomes.ProjectIdAlreadyExists));
        }

        [Fact]
        public async void ShouldNotAllowProjectsWithMultipleNamesForSameUserId()
        {
            var userId = Guid.NewGuid().ToString();
            _fixture.ProfileFactory.SetUserProfile(new Profile(userId));
            var createUserProject = _fixture.Container
                .Resolve<IServiceOpt<CreateNewProjectArg, CreateNewProjectResult,
                    CreateUserProjectOutcomes>>();

            var firstItem = new ProjectItem(Guid.NewGuid().ToString(), "Project 1");
            var secondItem = new ProjectItem(Guid.NewGuid().ToString(), "Project 1");

            var firstCreateOption = await
                createUserProject.Run(new CreateNewProjectArg(firstItem.Id, firstItem.Name));

            firstCreateOption.HasValue.Should().BeTrue();

            var secondCreateOption =
                await createUserProject.Run(new CreateNewProjectArg(secondItem.Id, secondItem.Name));

            secondCreateOption.Match(some => Assert.False(true, "Should Fail."),
                outcomes => outcomes.Should().Be(CreateUserProjectOutcomes.ProjectNameAlreadyExists));
        }

        [Fact]
        public async void ShouldPersistProjectsAndQueryThem()
        {
            var userId = Guid.NewGuid().ToString();
            _fixture.ProfileFactory.SetUserProfile(new Profile(userId));
            var createUserProject = _fixture.Container
                .Resolve<IServiceOpt<CreateNewProjectArg, CreateNewProjectResult,
                    CreateUserProjectOutcomes>>();

            var queryProject = _fixture.Container.Resolve<IQueryHandler<GetProject, GetProjectResult>>();

            var userProjects =
                _fixture.Container.Resolve<IQueryHandler<GetProjects, GetProjectsResult>>();

            var listOfProjectsToCreate = new List<ProjectItem>();
            listOfProjectsToCreate.Add(new ProjectItem(Guid.NewGuid().ToString(), "Project 1"));
            listOfProjectsToCreate.Add(new ProjectItem(Guid.NewGuid().ToString(), "Project 2"));
            listOfProjectsToCreate.Add(new ProjectItem(Guid.NewGuid().ToString(), "Project 3"));

            foreach (var item in listOfProjectsToCreate)
            {
                var optionResult = await
                    createUserProject.Run(new CreateNewProjectArg(item.Id, item.Name));

                optionResult.HasValue.Should().BeTrue("Failed To Create Project");
            }

            foreach (var item in listOfProjectsToCreate)
            {
                var optionQuery = await
                    queryProject.Run(new GetProject(item.Id));

                optionQuery.HasValue.Should().BeTrue();
            }

            var userProjectsOption = await userProjects.Run(new GetProjects(userId));
            userProjectsOption.HasValue.Should().BeTrue();

            userProjectsOption.MatchSome(x => x.Projects.Count().Should().Be(3));
        }
    }
}