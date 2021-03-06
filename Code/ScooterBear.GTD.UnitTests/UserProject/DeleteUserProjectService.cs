﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.UnitTests.UserProject
{
    public class AsTheDeleteUserProjectServiceI : IClassFixture<DeleteUserFixture>
    {
        public AsTheDeleteUserProjectServiceI(DeleteUserFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        private readonly DeleteUserFixture _fixture;

        [Fact]
        public async void ShouldReturnNotFoundIfDoesNotExist()
        {
            _fixture.ProjectQueryMock.Setup(x => x.Run(It.IsAny<GetProject>()))
                .Returns(Task.FromResult(Option.None<GetProjectResult>()));

            var optionResult = await _fixture.DeleteUserService.Run(new DeleteProjectArg("asdf"));

            optionResult.Match(some => Assert.False(true, "Should be Null"),
                outcome => outcome.Should().Be(DeleteProjectOutcome.NotFound));
        }

        [Fact]
        public async void ShouldReturnUnauthorizedIfDataAndProfileMismatch()
        {
            var projectId = Guid.NewGuid().ToString();
            var user = Guid.NewGuid().ToString();
            _fixture.ProfileFactoryMock.Setup(x => x.GetCurrentProfile())
                .Returns(new Application.UserProfile.Profile(user));

            var project = new Project(projectId, "name", Guid.NewGuid().ToString(), 0, 0, DateTime.UtcNow);

            _fixture.ProjectQueryMock.Setup(x => x.Run(It.IsAny<GetProject>()))
                .Returns(Task.FromResult(Option.Some(new GetProjectResult(project))));

            var optionResult = await
                _fixture.DeleteUserService.Run(new DeleteProjectArg(projectId));

            optionResult.Match(some => Assert.False(true, "Should be Null"),
                outcome => outcome.Should().Be(DeleteProjectOutcome.NotAuthorized));
        }
    }

    public class DeleteUserFixture : IDisposable
    {
        public DeleteUserFixture()
        {
            ProfileFactoryMock = new Mock<IProfileFactory>();
            ProjectQueryMock = new Mock<IQueryHandler<GetProject, GetProjectResult>>();
            PersistProjectMock =
                new Mock<IServiceOpt<PersistUpdateProjectArg, PersistUpdateProjectResult,
                    PersistUpdateProjectOutcome>>();
            DeleteUserService = new DeleteProject(ProfileFactoryMock.Object, ProjectQueryMock.Object,
                PersistProjectMock.Object);
        }

        public Mock<IServiceOpt<PersistUpdateProjectArg, PersistUpdateProjectResult,
            PersistUpdateProjectOutcome>> PersistProjectMock { get; }

        public Mock<IQueryHandler<GetProject, GetProjectResult>> ProjectQueryMock { get; }
        public Mock<IProfileFactory> ProfileFactoryMock { get; }
        public DeleteProject DeleteUserService { get; }

        public void Dispose()
        {
        }
    }
}