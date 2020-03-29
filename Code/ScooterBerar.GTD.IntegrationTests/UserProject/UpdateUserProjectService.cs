using System;
using System.Collections.Generic;
using Autofac;
using FluentAssertions;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.IntegrationTests.UserProject
{
    [Collection("DynamoDbDockerTests")]
    public class AsTheUpdateUserProjectServiceI
    {
        public AsTheUpdateUserProjectServiceI(RunDynamoDbDockerFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        private readonly RunDynamoDbDockerFixture _fixture;

        [Fact]
        public async void ShouldReturnNotFound()
        {
            var userId = Guid.NewGuid().ToString();
            _fixture.ProfileFactory.SetUserProfile(new Profile(userId));
            var service = _fixture.Container.Resolve<IServiceOpt<UpdateProjectNameArg,
                UpdateProjectNameResult, UpdateProjectOutcome>>();

            var updateOptionResult = await
                service.Run(new UpdateProjectNameArg(Guid.NewGuid().ToString(), "Project No Exist", 0));

            updateOptionResult.Match(some => Assert.False(true, "Should not FIne"),
                outcome => outcome.Should().Be(UpdateProjectOutcome.DoesNotExist));
        }

        [Fact]
        public async void ShouldUpdateValues()
        {
            var userId = Guid.NewGuid().ToString();
            _fixture.ProfileFactory.SetUserProfile(new Profile(userId));
            var service = _fixture.Container.Resolve<IServiceOpt<UpdateProjectNameArg,
                UpdateProjectNameResult, UpdateProjectOutcome>>();

            var createUserProject = _fixture.Container
                .Resolve<IServiceOpt<CreateNewProjectArg, CreateNewProjectResult,
                    CreateProjectOutcomes>>();

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

            for (var i = 0; i < listOfProjectsToCreate.Count; i++)
            {
                var updateOptionResult = await
                    service.Run(new UpdateProjectNameArg(listOfProjectsToCreate[i].Id, $"Project {i + 4}", 0));

                updateOptionResult.Match(some =>
                {
                    some.Project.Name.Should().Be($"Project {i + 4}");
                    some.Project.Id.Should().Be(listOfProjectsToCreate[i].Id);
                    some.Project.UserId.Should().Be(userId);
                }, outcome => Assert.True(false, $"Failed To Update Project: {outcome.ToString()}"));
            }
        }
    }
}