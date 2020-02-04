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
using ScooterBear.GTD.AWS.DynamoDb.Project;

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
                x.Run(It.IsAny<GetUserProjectQuery>()))
                .Returns(Task.FromResult(Option.None<GetUserProjectQueryResult>()));

            fixture.PersistService.Setup(x => 
                    x.Run(It.IsAny<PersistNewProjectServiceArg>()))
                .Returns(Task.FromResult(new PersistNewProjectServiceResult(fixture.Project)));

            var returnOption = await
                fixture.Service.Run(
                    new CreateNewUserProjectServiceArg("Id", "UserId", "Project"));

            returnOption.HasValue.Should().BeTrue();
        }

        [Fact]
        public async void ShouldFailIfDuplicateNameExists()
        {
            var fixture = CreateNewUserProjectFixture;

            fixture.Query.Setup(x =>
                    x.Run(It.IsAny<GetUserProjectQuery>()))
                .Returns(Task.FromResult(Option.Some(new GetUserProjectQueryResult(new UserProjects("UserId", new []{fixture.Project} )))));

            var returnOption = await
                fixture.Service.Run(
                    new CreateNewUserProjectServiceArg("IdTwo", "UserId", "ProjectName"));

            returnOption.HasValue.Should().BeFalse();
            returnOption.MatchNone(none=> none.Should().Be(CreateUserProjectOutcomes.ProjectNameAlreadyExists));
        }

        [Fact]
        public async void ShouldFailIfIdExists()
        {
            var fixture = CreateNewUserProjectFixture;

            fixture.Query.Setup(x =>
                    x.Run(It.IsAny<GetUserProjectQuery>()))
                .Returns(Task.FromResult(Option.Some(new GetUserProjectQueryResult(new UserProjects("UserId", new[] { fixture.Project })))));

            var returnOption = await
                fixture.Service.Run(
                    new CreateNewUserProjectServiceArg("Id", "UserId", "ProjectTwo"));

            returnOption.HasValue.Should().BeFalse();
            returnOption.MatchNone(none => none.Should().Be(CreateUserProjectOutcomes.ProjectIdAlreadyExists));
        }

        [Fact]
        public async void ShouldCreateWhenOtherProjectsExist()
        {
            var fixture = CreateNewUserProjectFixture;

            fixture.Query.Setup(x =>
                    x.Run(It.IsAny<GetUserProjectQuery>()))
                .Returns(Task.FromResult(Option.Some(new GetUserProjectQueryResult(new UserProjects("UserId", new[] { fixture.Project })))));

            var returnOption = await
                fixture.Service.Run(
                    new CreateNewUserProjectServiceArg("IdTwo", "UserId", "ProjectTwo"));

            returnOption.HasValue.Should().BeTrue();
        }
    }

    public class CreateNewUserProjectFixture : IDisposable
    {
        public readonly Mock<IQueryHandler<GetUserProjectQuery, GetUserProjectQueryResult>> Query;
        public readonly Mock<IService<PersistNewProjectServiceArg, PersistNewProjectServiceResult>> PersistService;
        public readonly Mock<IKnowTheDate> IKnowTheDate;
        public readonly CreateNewUserProjectService Service;
        public readonly ReadOnlyProject Project;
        public CreateNewUserProjectFixture()
        {
            Query = new Mock<IQueryHandler<GetUserProjectQuery, GetUserProjectQueryResult>>();
            PersistService = new Mock<IService<PersistNewProjectServiceArg, PersistNewProjectServiceResult>>();
            IKnowTheDate = new Mock<IKnowTheDate>();
            IKnowTheDate.Setup(x => x.UtcNow()).Returns(DateTime.UtcNow);
            Service = new CreateNewUserProjectService(Query.Object, PersistService.Object, IKnowTheDate.Object);
            Project = new ReadOnlyProject("Id", "UserId", "ProjectName", 0, false, 0, 0, DateTime.UtcNow);
        }
        
        public void Dispose()
        {
            
        }
    }
}
