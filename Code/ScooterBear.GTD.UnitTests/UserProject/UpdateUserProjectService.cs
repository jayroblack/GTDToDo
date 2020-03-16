using System;
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
        private readonly UpdateUserProjectFixture _fixture;

        public AsTheUpdateUserProjectService(UpdateUserProjectFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async void ShouldReturnNotFoundIfDoesNotExist()
        {
            var option = Option.None<ProjectQueryResult>();
            _fixture.MockedGetProject.Setup(x =>
                x.Run(It.IsAny<ProjectQuery>())).Returns(Task.FromResult(option));

            var testOptionResult = await
                this._fixture.UpdateUserProjectService.Run(_fixture.UpdateUserProjectServiceArg);
            testOptionResult.Match(some => Assert.False(true, "Should Return None"),
                outcome => outcome.Should().Be(UpdateProjectOutcome.DoesNotExist));
        }

        [Theory]
        [InlineData(null, "Name", 0, false, 0, false)]
        [InlineData("", "Name", 0, false, 0, false)]
        [InlineData("ProjectId", null, 0, false, 0, false)]
        [InlineData("ProjectId", "", 0, false, 0, false)]
        [InlineData("ProjectId", "Name", -1, false, 0, false)]
        [InlineData("ProjectId", "Name", 0, false, -1, false)]
        [InlineData("ProjectId", "Name", 0, false, 1, true)]
        public async void ShouldReturnUnprocessableEntityIfValidationFails(string projectId, string projectName,
            int count, bool isDeleted, int countOverdue, bool shouldPass)
        {
            var userId = Guid.NewGuid().ToString();
            var project = new Project(projectId, projectName,
                userId, count, isDeleted, count, 0, DateTime.UtcNow);

            this._fixture.ProfileFactory.SetUserProfile(new Application.UserProfile.Profile(userId));
            var option = Option.Some<ProjectQueryResult>(new ProjectQueryResult(project));

            _fixture.MockedGetProject.Setup(x =>
                x.Run(It.IsAny<ProjectQuery>())).Returns(Task.FromResult(option));

            var persistOption =
                Option.Some<PersistUpdateProjectServiceResult, PersistUpdateProjectOutcome>(
                    new PersistUpdateProjectServiceResult(project));

            _fixture.MockedPersistProject.Setup(x =>
                    x.Run(It.IsAny<PersistUpdateProjectServiceArgs>()))
                .Returns(Task.FromResult(persistOption));

            var testOptionResult = await
                this._fixture.UpdateUserProjectService.Run(
                    new UpdateUserProjectServiceArg(projectId, projectName, count, isDeleted, countOverdue, 0));

            if (!shouldPass)
            {
                testOptionResult.Match(some => Assert.False(true, "Should Return None"),
                    outcome => outcome.Should().Be(UpdateProjectOutcome.UnprocessableEntity));
            }
            else
            {
                testOptionResult.Match(some => Assert.True(true),
                    outcome => Assert.False(true, "Should Not Fail."));
            }
        }

        [Fact]
        public async void ShouldReturnUnauthorizedIfDataAndProfileMismatch()
        {
            var userId = Guid.NewGuid().ToString();

            var project = new Project("ProjectId", "ProjectName",
                Guid.NewGuid().ToString(), 0, false, 2, 0, DateTime.UtcNow);

            this._fixture.ProfileFactory.SetUserProfile(new Application.UserProfile.Profile(userId));
            var option = Option.Some<ProjectQueryResult>(new ProjectQueryResult(project));

            _fixture.MockedGetProject.Setup(x =>
                x.Run(It.IsAny<ProjectQuery>())).Returns(Task.FromResult(option));

            var testOptionResult = await
                this._fixture.UpdateUserProjectService.Run(new UpdateUserProjectServiceArg(project.Id, project.Name,
                    project.Count, project.IsDeleted, project.CountOverDue, 0));

            testOptionResult.Match(some => Assert.False(true, "Should Return None"),
                outcome => outcome.Should().Be(UpdateProjectOutcome.NotAuthorized));
        }

        [Fact]
        public async void ShouldReturnConflictIfStaleRead()
        {
            var userId = Guid.NewGuid().ToString();

            var project = new Project("ProjectId", "ProjectName",
                userId, 0, false, 2, 0, DateTime.UtcNow);

            this._fixture.ProfileFactory.SetUserProfile(new Application.UserProfile.Profile(userId));
            var option = Option.Some<ProjectQueryResult>(new ProjectQueryResult(project));

            _fixture.MockedGetProject.Setup(x =>
                x.Run(It.IsAny<ProjectQuery>())).Returns(Task.FromResult(option));

            var persistOption =
                Option.None<PersistUpdateProjectServiceResult, PersistUpdateProjectOutcome>(PersistUpdateProjectOutcome
                    .Conflict);

            this._fixture.MockedPersistProject.Setup(x => x.Run(It.IsAny<PersistUpdateProjectServiceArgs>()))
                .Returns(Task.FromResult(persistOption));

            var testOptionResult = await
                this._fixture.UpdateUserProjectService.Run(new UpdateUserProjectServiceArg(project.Id, project.Name,
                    project.Count, project.IsDeleted, project.CountOverDue, 0));

            testOptionResult.Match(some => Assert.False(true, "Should Return None"),
                outcome => outcome.Should().Be(UpdateProjectOutcome.VersionConflict));
        }
    }

    public class UpdateUserProjectFixture : IDisposable
    {
        public Mock<IServiceOptOutcomes<PersistUpdateProjectServiceArgs, PersistUpdateProjectServiceResult,
            PersistUpdateProjectOutcome>> MockedPersistProject { get; private set; }

        public UpdateUserProjectService UpdateUserProjectService { get; private set; }
        public Mock<IQueryHandler<ProjectQuery, ProjectQueryResult>> MockedGetProject { get; private set; }
        public FakedProfileFactory ProfileFactory { get; private set; }
        public Mock<ILogger> MockedLogger { get; private set; }
        public UpdateUserProjectServiceArg UpdateUserProjectServiceArg { get; set; }

        public UpdateUserProjectFixture()
        {
            this.ProfileFactory = new FakedProfileFactory();
            this.MockedLogger = new Mock<ILogger>();
            this.MockedGetProject = new Mock<IQueryHandler<ProjectQuery, ProjectQueryResult>>();
            this.MockedPersistProject =
                new Mock<IServiceOptOutcomes<PersistUpdateProjectServiceArgs, PersistUpdateProjectServiceResult,
                    PersistUpdateProjectOutcome>>();
            this.UpdateUserProjectServiceArg = new UpdateUserProjectServiceArg("ProjectId", "Name", 1, false, 3, 0);
            this.UpdateUserProjectService = new UpdateUserProjectService(this.ProfileFactory, this.MockedLogger.Object,
                this.MockedGetProject.Object, this.MockedPersistProject.Object);
        }

        public void Dispose()
        {
        }
    }
}
