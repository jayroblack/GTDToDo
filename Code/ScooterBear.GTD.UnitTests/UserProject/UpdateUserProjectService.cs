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

            _fixture.MockedGetUserProjects.Setup(x => x.Run(It.IsAny<GetUserProjectsQuery>()))
                .Returns(Task.FromResult(Option.None<GetUserProjectsQueryResult>()));

            var testOptionResult = await
                this._fixture.UpdateUserProjectService.Run(_fixture.UpdateUserProjectServiceArg);

            testOptionResult.Match(some => Assert.False(true, "Should Return None"),
                outcome => outcome.Should().Be(UpdateProjectOutcome.DoesNotExist));
        }

        [Fact]
        public async void ShouldReturnUnauthorizedIfDataAndProfileMismatch()
        {
            var userId = Guid.NewGuid().ToString();

            var project = new Project("ProjectId", "ProjectName",
                Guid.NewGuid().ToString(), 0, 2, DateTime.UtcNow);

            this._fixture.ProfileFactory.SetUserProfile(new Application.UserProfile.Profile(userId));
            var option = Option.Some<ProjectQueryResult>(new ProjectQueryResult(project));

            _fixture.MockedGetProject.Setup(x =>
                x.Run(It.IsAny<ProjectQuery>())).Returns(Task.FromResult(option));

            _fixture.MockedGetUserProjects.Setup(x => x.Run(It.IsAny<GetUserProjectsQuery>()))
                .Returns(Task.FromResult(Option.None<GetUserProjectsQueryResult>()));

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
                userId, 0, 2, DateTime.UtcNow);

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
            PersistUpdateProjectOutcome>> MockedPersistProject { get; }
        public UpdateUserProjectService UpdateUserProjectService { get; }
        public Mock<IQueryHandler<ProjectQuery, ProjectQueryResult>> MockedGetProject { get; }
        public FakedProfileFactory ProfileFactory { get; }
        public Mock<ILogger> MockedLogger { get; }
        public UpdateUserProjectServiceArg UpdateUserProjectServiceArg { get; }
        public Mock<IQueryHandler<GetUserProjectsQuery, GetUserProjectsQueryResult>> MockedGetUserProjects { get; }

        public UpdateUserProjectFixture()
        {
            this.ProfileFactory = new FakedProfileFactory();
            this.MockedLogger = new Mock<ILogger>();
            this.MockedGetProject = new Mock<IQueryHandler<ProjectQuery, ProjectQueryResult>>();
            this.MockedPersistProject =
                new Mock<IServiceOptOutcomes<PersistUpdateProjectServiceArgs, PersistUpdateProjectServiceResult,
                    PersistUpdateProjectOutcome>>();
            this.UpdateUserProjectServiceArg = new UpdateUserProjectServiceArg("ProjectId", "Name", 1, false, 3, 0);
            this.MockedGetUserProjects = new Mock<IQueryHandler<GetUserProjectsQuery, GetUserProjectsQueryResult>>();
            this.UpdateUserProjectService = new UpdateUserProjectService(this.ProfileFactory, this.MockedLogger.Object,
                this.MockedGetProject.Object, this.MockedPersistProject.Object, this.MockedGetUserProjects.Object);
        }

        

        public void Dispose()
        {
        }
    }
}
