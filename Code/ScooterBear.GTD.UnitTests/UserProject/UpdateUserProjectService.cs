﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Fakes;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.UnitTests.UserProject
{
    public class AsTheUpdateUserProjectService : IClassFixture<UpdateUserProjectFixture>
    {
        public AsTheUpdateUserProjectService(UpdateUserProjectFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        private readonly UpdateUserProjectFixture _fixture;

        [Fact]
        public async void ShouldReturnConflictIfStaleRead()
        {
            var userId = Guid.NewGuid().ToString();

            var project = new Project("ProjectId", "ProjectName",
                userId, 0, 2, DateTime.UtcNow);

            _fixture.ProfileFactory.SetUserProfile(new Application.UserProfile.Profile(userId));
            var option = Option.Some(new GetProjectResult(project));

            _fixture.MockedGetProject.Setup(x =>
                x.Run(It.IsAny<GetProject>())).Returns(Task.FromResult(option));

            var persistOption =
                Option.None<PersistUpdateProjectResult, PersistUpdateProjectOutcome>(PersistUpdateProjectOutcome
                    .Conflict);

            _fixture.MockedPersistProject.Setup(x => x.Run(It.IsAny<PersistUpdateProjectArg>()))
                .Returns(Task.FromResult(persistOption));

            var testOptionResult = await
                _fixture.UpdateProject.Run(new UpdateProjectNameArg(project.Id, project.Name, 0));

            testOptionResult.Match(some => Assert.False(true, "Should Return None"),
                outcome => outcome.Should().Be(UpdateProjectOutcome.VersionConflict));
        }

        [Fact]
        public async void ShouldReturnNotFoundIfDoesNotExist()
        {
            var option = Option.None<GetProjectResult>();
            _fixture.MockedGetProject.Setup(x =>
                x.Run(It.IsAny<GetProject>())).Returns(Task.FromResult(option));

            _fixture.MockedGetUserProjects.Setup(x => x.Run(It.IsAny<GetProjects>()))
                .Returns(Task.FromResult(Option.None<GetProjectsResult>()));

            var testOptionResult = await
                _fixture.UpdateProject.Run(_fixture.UpdateProjectNameArg);

            testOptionResult.Match(some => Assert.False(true, "Should Return None"),
                outcome => outcome.Should().Be(UpdateProjectOutcome.DoesNotExist));
        }

        [Fact]
        public async void ShouldReturnUnauthorizedIfDataAndProfileMismatch()
        {
            var userId = Guid.NewGuid().ToString();

            var project = new Project("ProjectId", "ProjectName",
                Guid.NewGuid().ToString(), 0, 2, DateTime.UtcNow);

            _fixture.ProfileFactory.SetUserProfile(new Application.UserProfile.Profile(userId));
            var option = Option.Some(new GetProjectResult(project));

            _fixture.MockedGetProject.Setup(x =>
                x.Run(It.IsAny<GetProject>())).Returns(Task.FromResult(option));

            _fixture.MockedGetUserProjects.Setup(x => x.Run(It.IsAny<GetProjects>()))
                .Returns(Task.FromResult(Option.None<GetProjectsResult>()));

            var testOptionResult = await
                _fixture.UpdateProject.Run(new UpdateProjectNameArg(project.Id, project.Name, 0));

            testOptionResult.Match(some => Assert.False(true, "Should Return None"),
                outcome => outcome.Should().Be(UpdateProjectOutcome.NotAuthorized));
        }
    }

    public class UpdateUserProjectFixture : IDisposable
    {
        public UpdateUserProjectFixture()
        {
            ProfileFactory = new FakedProfileFactory();
            MockedLogger = new Mock<ILogger>();
            MockedGetProject = new Mock<IQueryHandler<GetProject, GetProjectResult>>();
            MockedPersistProject =
                new Mock<IServiceOpt<PersistUpdateProjectArg, PersistUpdateProjectResult,
                    PersistUpdateProjectOutcome>>();
            UpdateProjectNameArg = new UpdateProjectNameArg("ProjectId", "Name", 0);
            MockedGetUserProjects = new Mock<IQueryHandler<GetProjects, GetProjectsResult>>();
            UpdateProject = new UpdateProject(ProfileFactory, MockedLogger.Object,
                MockedGetProject.Object, MockedPersistProject.Object, MockedGetUserProjects.Object);
        }

        public Mock<IServiceOpt<PersistUpdateProjectArg, PersistUpdateProjectResult,
            PersistUpdateProjectOutcome>> MockedPersistProject { get; }

        public UpdateProject UpdateProject { get; }
        public Mock<IQueryHandler<GetProject, GetProjectResult>> MockedGetProject { get; }
        public FakedProfileFactory ProfileFactory { get; }
        public Mock<ILogger> MockedLogger { get; }
        public UpdateProjectNameArg UpdateProjectNameArg { get; }
        public Mock<IQueryHandler<GetProjects, GetProjectsResult>> MockedGetUserProjects { get; }


        public void Dispose()
        {
        }
    }
}