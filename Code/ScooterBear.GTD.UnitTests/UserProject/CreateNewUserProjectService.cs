using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;
using Xunit;
using Optional;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.AWS.DynamoDb.Projects;

namespace ScooterBear.GTD.UnitTests.UserProject
{
    public class AsTheCreateNewUserProjectServiceI : IClassFixture<CreateNewUserProjectFixture>
    {
        public CreateNewUserProjectFixture CreateNewUserProjectFixture { get; }

        public AsTheCreateNewUserProjectServiceI(
            CreateNewUserProjectFixture createNewUserProjectFixture)
        {
            CreateNewUserProjectFixture = createNewUserProjectFixture;
        }

        [Fact]
        public async void ShouldCreateWhenNoOtherProjectsExist()
        {
            var fixture = CreateNewUserProjectFixture;

            fixture.Query.Setup(x=> 
                x.Run(It.IsAny<GetUserProjectsQuery>()))
                .Returns(Task.FromResult(Option.None<GetUserProjectsQueryResult>()));

            fixture.PersistService.Setup(x => 
                    x.Run(It.IsAny<PersistNewProjectServiceArg>()))
                .Returns(Task.FromResult(new PersistNewProjectServiceResult(fixture.Project)));

            var returnOption = await
                fixture.Service.Run(
                    new CreateNewUserProjectServiceArg("Id", "Project"));

            returnOption.HasValue.Should().BeTrue();
        }

        [Fact]
        public async void ShouldFailIfDuplicateNameExists()
        {
            var fixture = CreateNewUserProjectFixture;

            fixture.Query.Setup(x =>
                    x.Run(It.IsAny<GetUserProjectsQuery>()))
                .Returns(Task.FromResult(Option.Some(new GetUserProjectsQueryResult(new UserProjects("UserId", new []{fixture.Project} )))));

            var returnOption = await
                fixture.Service.Run(
                    new CreateNewUserProjectServiceArg("IdTwo", "ProjectName"));

            returnOption.HasValue.Should().BeFalse();
            returnOption.MatchNone(none=> none.Should().Be(CreateUserProjectOutcomes.ProjectNameAlreadyExists));
        }

        [Fact]
        public async void ShouldFailIfIdExists()
        {
            var fixture = CreateNewUserProjectFixture;

            fixture.Query.Setup(x =>
                    x.Run(It.IsAny<GetUserProjectsQuery>()))
                .Returns(Task.FromResult(Option.Some(new GetUserProjectsQueryResult(new UserProjects("UserId", new[] { fixture.Project })))));

            var returnOption = await
                fixture.Service.Run(
                    new CreateNewUserProjectServiceArg("Id", "ProjectTwo"));

            returnOption.HasValue.Should().BeFalse();
            returnOption.MatchNone(none => none.Should().Be(CreateUserProjectOutcomes.ProjectIdAlreadyExists));
        }

        [Fact]
        public async void ShouldCreateWhenOtherProjectsExist()
        {
            var fixture = CreateNewUserProjectFixture;

            fixture.Query.Setup(x =>
                    x.Run(It.IsAny<GetUserProjectsQuery>()))
                .Returns(Task.FromResult(Option.Some(new GetUserProjectsQueryResult(new UserProjects("UserId", new[] { fixture.Project })))));

            var returnOption = await
                fixture.Service.Run(
                    new CreateNewUserProjectServiceArg("IdTwo","ProjectTwo"));

            returnOption.HasValue.Should().BeTrue();
        }
    }

    public class CreateNewUserProjectFixture : IDisposable
    {
        public readonly Mock<IQueryHandler<GetUserProjectsQuery, GetUserProjectsQueryResult>> Query;
        public readonly Mock<IService<PersistNewProjectServiceArg, PersistNewProjectServiceResult>> PersistService;
        public readonly Mock<IKnowTheDate> IKnowTheDate;
        public readonly CreateNewUserProjectService Service;
        public readonly ReadOnlyProject Project;
        public readonly Mock<IProfileFactory> UserProfileFactory;

        public CreateNewUserProjectFixture()
        {
            Query = new Mock<IQueryHandler<GetUserProjectsQuery, GetUserProjectsQueryResult>>();
            PersistService = new Mock<IService<PersistNewProjectServiceArg, PersistNewProjectServiceResult>>();
            IKnowTheDate = new Mock<IKnowTheDate>();
            IKnowTheDate.Setup(x => x.UtcNow()).Returns(DateTime.UtcNow);
            this.UserProfileFactory = new Mock<IProfileFactory>();
            var userProfile = new Application.UserProfile.Profile(Guid.NewGuid().ToString());
            this.UserProfileFactory.Setup(x => x.GetCurrentProfile()).Returns(userProfile);
            Service = new CreateNewUserProjectService(Query.Object, PersistService.Object, 
                IKnowTheDate.Object, this.UserProfileFactory.Object);
            Project = new ReadOnlyProject("Id", "UserId", "ProjectName", 0, false, 0, 0, DateTime.UtcNow);
        }

        

        public void Dispose()
        {
            
        }
    }
}
