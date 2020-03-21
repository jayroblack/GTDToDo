using System;
using System.Linq;
using System.Threading.Tasks;
using Optional;
using Optional.Async.Extensions;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public enum CreateUserProjectOutcomes
    {
        ProjectNameAlreadyExists,
        ProjectIdAlreadyExists
    }

    public class CreateNewProject : IServiceOpt<CreateNewProjectArg, CreateNewProjectResult,
        CreateUserProjectOutcomes>
    {
        private readonly IKnowTheDate _iKnowTheDate;
        private readonly IService<PersistNewProjectArg, PersistNewProjectResult> _persistProject;
        private readonly IProfileFactory _profileFactory;
        private readonly IQueryHandler<GetProjects, GetProjectsResult> _getProjects;

        public CreateNewProject(
            IQueryHandler<GetProjects, GetProjectsResult> getProjects,
            IService<PersistNewProjectArg, PersistNewProjectResult> persistProject,
            IKnowTheDate iKnowTheDate,
            IProfileFactory profileFactory)
        {
            _getProjects = getProjects ?? throw new ArgumentNullException(nameof(getProjects));
            _persistProject = persistProject ?? throw new ArgumentNullException(nameof(persistProject));
            _iKnowTheDate = iKnowTheDate ?? throw new ArgumentNullException(nameof(iKnowTheDate));
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
        }

        public async Task<Option<CreateNewProjectResult, CreateUserProjectOutcomes>> Run(CreateNewProjectArg arg)
        {
            var userId = _profileFactory.GetCurrentProfile().UserId;

            var userProjects = await _getProjects.Run(new GetProjects(userId));

            var dateTimeCreated = _iKnowTheDate.UtcNow();

            return await
                userProjects.MatchAsync(async some =>
                    {
                        //Project Names Must be Unique
                        if (some.Projects.Any(x => x.Name == arg.NewProjectName && !x.IsDeleted))
                            return Option.None<CreateNewProjectResult, CreateUserProjectOutcomes>(
                                CreateUserProjectOutcomes.ProjectNameAlreadyExists);

                        //Project Ids Must be Unique
                        if (some.Projects.Any(x => x.Id == arg.Id))
                            return Option.None<CreateNewProjectResult, CreateUserProjectOutcomes>(
                                CreateUserProjectOutcomes.ProjectIdAlreadyExists);

                        var persistResult = await
                            _persistProject.Run(new PersistNewProjectArg(arg.Id, userId, arg.NewProjectName,
                                dateTimeCreated));

                        return Option.Some<CreateNewProjectResult, CreateUserProjectOutcomes>(
                            new CreateNewProjectResult(persistResult.Project));
                    },
                    async () =>
                    {
                        var persistResult = await
                            _persistProject.Run(new PersistNewProjectArg(arg.Id, userId, arg.NewProjectName,
                                dateTimeCreated, arg.ConsistentRead));

                        return Option.Some<CreateNewProjectResult, CreateUserProjectOutcomes>(
                            new CreateNewProjectResult(persistResult.Project));
                    });
        }
    }
}