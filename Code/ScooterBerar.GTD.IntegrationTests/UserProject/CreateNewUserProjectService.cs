using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using FluentAssertions;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.IntegrationTests.UserProject
{
    [Collection("DynamoDbDockerTests")]
    public class AsTheCreateNewUserProjectServiceI
    {
        private readonly RunDynamoDbDockerFixture _fixture;

        public AsTheCreateNewUserProjectServiceI(RunDynamoDbDockerFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        private class ProjectItem
        {
            public string Id { get; }
            public string Name { get; }

            public ProjectItem(string id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        [Fact]
        public async void ShouldPersistProjectsAndQueryThem()
        {
            var userId = Guid.NewGuid().ToString();
            var createUserProject = _fixture.Container
                .Resolve<IServiceOptOutcomes<CreateNewUserProjectServiceArg, CreateNewUserProjectServiceResult,
                    CreateUserProjectOutcomes>>();

            var queryProject = _fixture.Container.Resolve<IQueryHandler<ProjectQuery, ProjectQueryResult>>();

            var userProjects  =
                _fixture.Container.Resolve<IQueryHandler<GetUserProjectsQuery, GetUserProjectsQueryResult>>();

            var listOfProjectsToCreate = new List<ProjectItem>();
            listOfProjectsToCreate.Add(new ProjectItem(Guid.NewGuid().ToString(), "Project 1"));
            listOfProjectsToCreate.Add(new ProjectItem(Guid.NewGuid().ToString(), "Project 2"));
            listOfProjectsToCreate.Add(new ProjectItem(Guid.NewGuid().ToString(), "Project 3"));

            foreach (var item in listOfProjectsToCreate)
            {
                var optionResult = await
                    createUserProject.Run(new CreateNewUserProjectServiceArg(item.Id, userId, item.Name));

                optionResult.HasValue.Should().BeTrue("Failed To Create Project");
            }

            foreach (var item in listOfProjectsToCreate)
            {
                var optionQuery = await
                    queryProject.Run(new ProjectQuery(item.Id));

                optionQuery.HasValue.Should().BeTrue();
            }

            var userProjectsOption = await userProjects.Run(new GetUserProjectsQuery(userId));
            userProjectsOption.HasValue.Should().BeTrue();

            userProjectsOption.MatchSome(x => x.UserProjects.Projects.Count().Should().Be(3));
        }

        [Fact]
        public async void ShouldNotAllowProjectsWithMultipleNamesForSameUserId()
        {
            var userId = Guid.NewGuid().ToString();
            var createUserProject = _fixture.Container
                .Resolve<IServiceOptOutcomes<CreateNewUserProjectServiceArg, CreateNewUserProjectServiceResult,
                    CreateUserProjectOutcomes>>();

            var firstItem = new ProjectItem(Guid.NewGuid().ToString(), "Project 1");
            var secondItem = new ProjectItem(Guid.NewGuid().ToString(), "Project 1");

            var firstCreateOption = await 
                createUserProject.Run(new CreateNewUserProjectServiceArg(firstItem.Id, userId, firstItem.Name));

            firstCreateOption.HasValue.Should().BeTrue();

            var secondCreateOption =
                await createUserProject.Run(new CreateNewUserProjectServiceArg(secondItem.Id, userId, secondItem.Name));

            secondCreateOption.Match(some => Assert.False(true, "Should Fail."),
                outcomes => outcomes.Should().Be(CreateUserProjectOutcomes.ProjectNameAlreadyExists));
        }

        [Fact]
        public async void ShouldNotAllowANewProjectToBeCreatedWithAnExistingId()
        {
            var userId = Guid.NewGuid().ToString();
            var createUserProject = _fixture.Container
                .Resolve<IServiceOptOutcomes<CreateNewUserProjectServiceArg, CreateNewUserProjectServiceResult,
                    CreateUserProjectOutcomes>>();

            var id = Guid.NewGuid().ToString();
            var firstItem = new ProjectItem(id, "Project 1");
            var secondItem = new ProjectItem(id, "Project 2");

            var firstCreateOption = await
                createUserProject.Run(new CreateNewUserProjectServiceArg(firstItem.Id, userId, firstItem.Name));

            firstCreateOption.HasValue.Should().BeTrue();

            var secondCreateOption =
                await createUserProject.Run(new CreateNewUserProjectServiceArg(secondItem.Id, userId, secondItem.Name));

            secondCreateOption.Match(some => Assert.False(true, "Should Fail."),
                outcomes => outcomes.Should().Be(CreateUserProjectOutcomes.ProjectIdAlreadyExists));
        }
    }
}
