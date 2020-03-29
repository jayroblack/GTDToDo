using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.AWS.DynamoDb.Projects;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;

namespace ScooterBear.GTD.UnitTests.UserProject
{
    public class AsTheCreateNewUserProjectServiceI : IClassFixture<CreateNewUserProjectFixture>
    {
        public AsTheCreateNewUserProjectServiceI(
            CreateNewUserProjectFixture createNewUserProjectFixture)
        {
            CreateNewUserProjectFixture = createNewUserProjectFixture;
        }

        public CreateNewUserProjectFixture CreateNewUserProjectFixture { get; }

        [Fact]
        public async void ShouldCreateWhenNoOtherProjectsExist()
        {
            var fixture = CreateNewUserProjectFixture;

            fixture.Query.Setup(x =>
                    x.Run(It.IsAny<GetProjects>()))
                .Returns(Task.FromResult(Option.None<GetProjectsResult>()));

            fixture.PersistService.Setup(x =>
                    x.Run(It.IsAny<PersistProjectArg>()))
                .Returns(Task.FromResult(new PersistNewProjectResult(fixture.Project)));

            var returnOption = await
                fixture.Service.Run(
                    new CreateNewProjectArg("Id", "Project"));

            returnOption.HasValue.Should().BeTrue();
        }

        [Fact]
        public async void ShouldCreateWhenOtherProjectsExist()
        {
            var fixture = CreateNewUserProjectFixture;

            fixture.Query.Setup(x =>
                    x.Run(It.IsAny<GetProjects>()))
                .Returns(Task.FromResult(Option.Some(new GetProjectsResult("UserId", new[] {fixture.Project}))));

            var returnOption = await
                fixture.Service.Run(
                    new CreateNewProjectArg("IdTwo", "ProjectTwo"));

            returnOption.HasValue.Should().BeTrue();
        }

        [Fact]
        public async void ShouldFailIfDuplicateNameExists()
        {
            var fixture = CreateNewUserProjectFixture;

            fixture.Query.Setup(x =>
                    x.Run(It.IsAny<GetProjects>()))
                .Returns(Task.FromResult(Option.Some(new GetProjectsResult("UserId", new[] {fixture.Project}))));

            var returnOption = await
                fixture.Service.Run(
                    new CreateNewProjectArg("IdTwo", "ProjectName"));

            returnOption.HasValue.Should().BeFalse();
            returnOption.MatchNone(none => none.Should().Be(CreateProjectOutcomes.ProjectNameAlreadyExists));
        }

        [Fact]
        public async void ShouldFailIfIdExists()
        {
            var fixture = CreateNewUserProjectFixture;

            fixture.Query.Setup(x =>
                    x.Run(It.IsAny<GetProjects>()))
                .Returns(Task.FromResult(Option.Some(new GetProjectsResult("UserId", new[] {fixture.Project}))));

            var returnOption = await
                fixture.Service.Run(
                    new CreateNewProjectArg("Id", "ProjectTwo"));

            returnOption.HasValue.Should().BeFalse();
            returnOption.MatchNone(none => none.Should().Be(CreateProjectOutcomes.ProjectIdAlreadyExists));
        }
    }

    public class CreateNewUserProjectFixture : IDisposable
    {
        public readonly Mock<IKnowTheDate> IKnowTheDate;
        public readonly Mock<IService<PersistProjectArg, PersistNewProjectResult>> PersistService;
        public readonly ReadOnlyProject Project;
        public readonly Mock<IQueryHandler<GetProjects, GetProjectsResult>> Query;
        public readonly CreateNewProject Service;
        public readonly Mock<IProfileFactory> UserProfileFactory;

        public CreateNewUserProjectFixture()
        {
            Query = new Mock<IQueryHandler<GetProjects, GetProjectsResult>>();
            PersistService = new Mock<IService<PersistProjectArg, PersistNewProjectResult>>();
            IKnowTheDate = new Mock<IKnowTheDate>();
            IKnowTheDate.Setup(x => x.UtcNow()).Returns(DateTime.UtcNow);
            UserProfileFactory = new Mock<IProfileFactory>();
            var userProfile = new Application.UserProfile.Profile(Guid.NewGuid().ToString());
            UserProfileFactory.Setup(x => x.GetCurrentProfile()).Returns(userProfile);
            Service = new CreateNewProject(Query.Object, PersistService.Object,
                IKnowTheDate.Object, UserProfileFactory.Object);
            Project = new ReadOnlyProject("Id", "UserId", "ProjectName", 0, false, 0, 0, DateTime.UtcNow);
        }


        public void Dispose()
        {
        }
    }
}