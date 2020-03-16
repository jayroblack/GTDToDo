using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Optional;
using Optional.Async.Extensions;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.UnitTests.UserProject
{
    public class AsTheDeleteUserProjectServiceI : IClassFixture<DeleteUserFixture>
    {
        private readonly DeleteUserFixture _fixture;

        public AsTheDeleteUserProjectServiceI(DeleteUserFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async void ShouldReturnNotFoundIfDoesNotExist()
        {
            _fixture.ProjectQueryMock.Setup(x => x.Run(It.IsAny<ProjectQuery>()))
                .Returns(Task.FromResult(Option.None<ProjectQueryResult>()));

            var optionResult = await _fixture.DeleteUserService.Run(new DeleteUserProjectServiceArgs("asdf"));

            optionResult.Match(some => Assert.False(true, "Should be Null"),
                outcome => outcome.Should().Be(DeleteUserProjectOutcome.NotFound));
        }

        [Fact]
        public async void ShouldReturnUnauthorizedIfDataAndProfileMismatch()
        {
            var projectId = Guid.NewGuid().ToString();
            var user = Guid.NewGuid().ToString();
            _fixture.ProfileFactoryMock.Setup(x => x.GetCurrentProfile())
                .Returns(new Application.UserProfile.Profile(user));

            var project = new Project(projectId, "name", Guid.NewGuid().ToString(), 0, false, 0, 0, DateTime.UtcNow);

            _fixture.ProjectQueryMock.Setup(x => x.Run(It.IsAny<ProjectQuery>()))
                .Returns(Task.FromResult(Option.Some<ProjectQueryResult>(new ProjectQueryResult(project))));

            var optionResult = await
                _fixture.DeleteUserService.Run(new DeleteUserProjectServiceArgs(projectId));
            
            optionResult.Match(some => Assert.False(true, "Should be Null"),
                outcome => outcome.Should().Be(DeleteUserProjectOutcome.NotAuthorized));
        }
    }

    public class DeleteUserFixture : IDisposable
    {
        public Mock<IServiceOptOutcomes<PersistUpdateProjectServiceArgs, PersistUpdateProjectServiceResult, PersistUpdateProjectOutcome>> PersistProjectMock { get; }
        public Mock<IQueryHandler<ProjectQuery, ProjectQueryResult>> ProjectQueryMock { get; }
        public Mock<ILogger> LoggerMock { get; }
        public Mock<IProfileFactory> ProfileFactoryMock { get; }
        public DeleteUserProjectService DeleteUserService { get; }

        public DeleteUserFixture()
        {
            this.ProfileFactoryMock = new Mock<IProfileFactory>();
            this.LoggerMock = new Mock<ILogger>();
            this.ProjectQueryMock = new Mock<IQueryHandler<ProjectQuery, ProjectQueryResult>>();
            this.PersistProjectMock = new Mock<IServiceOptOutcomes<PersistUpdateProjectServiceArgs, PersistUpdateProjectServiceResult, PersistUpdateProjectOutcome>>();
            this.DeleteUserService = new DeleteUserProjectService(this.ProfileFactoryMock.Object, this.LoggerMock.Object, this.ProjectQueryMock.Object, this.PersistProjectMock.Object);
        }
        
        public void Dispose()
        {

        }
    }
}
