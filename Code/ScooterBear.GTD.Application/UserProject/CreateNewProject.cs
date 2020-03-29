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
    public enum CreateProjectOutcomes
    {
        ProjectNameAlreadyExists,
        ProjectIdAlreadyExists
    }

    public class CreateNewProject : IServiceOpt<CreateNewProjectArg, CreateNewProjectResult,
        CreateProjectOutcomes>
    {
        private readonly IQueryHandler<GetProjects, GetProjectsResult> _getProjects;
        private readonly IKnowTheDate _iKnowTheDate;
        private readonly IService<PersistProjectArg, PersistNewProjectResult> _persistProject;
        private readonly IProfileFactory _profileFactory;

        public CreateNewProject(
            IQueryHandler<GetProjects, GetProjectsResult> getProjects,
            IService<PersistProjectArg, PersistNewProjectResult> persistProject,
            IKnowTheDate iKnowTheDate,
            IProfileFactory profileFactory)
        {
            _getProjects = getProjects ?? throw new ArgumentNullException(nameof(getProjects));
            _persistProject = persistProject ?? throw new ArgumentNullException(nameof(persistProject));
            _iKnowTheDate = iKnowTheDate ?? throw new ArgumentNullException(nameof(iKnowTheDate));
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
        }

        public async Task<Option<CreateNewProjectResult, CreateProjectOutcomes>> Run(CreateNewProjectArg arg)
        {
            var userId = _profileFactory.GetCurrentProfile().UserId;

            var projects = await _getProjects.Run(new GetProjects(userId));

            var dateTimeCreated = _iKnowTheDate.UtcNow();

            return await
                projects.MatchAsync(async some =>
                    {
                        if (some.Projects.Any(x => x.Name == arg.NewProjectName && !x.IsDeleted))
                            return Option.None<CreateNewProjectResult, CreateProjectOutcomes>(
                                CreateProjectOutcomes.ProjectNameAlreadyExists);

                        if (some.Projects.Any(x => x.Id == arg.Id))
                            return Option.None<CreateNewProjectResult, CreateProjectOutcomes>(
                                CreateProjectOutcomes.ProjectIdAlreadyExists);

                        var persistResult = await
                            _persistProject.Run(new PersistProjectArg(arg.Id, userId, arg.NewProjectName,
                                dateTimeCreated));

                        return Option.Some<CreateNewProjectResult, CreateProjectOutcomes>(
                            new CreateNewProjectResult(persistResult.Project));
                    },
                    async () =>
                    {
                        var persistResult = await
                            _persistProject.Run(new PersistProjectArg(arg.Id, userId, arg.NewProjectName,
                                dateTimeCreated, arg.ConsistentRead));

                        return Option.Some<CreateNewProjectResult, CreateProjectOutcomes>(
                            new CreateNewProjectResult(persistResult.Project));
                    });
        }
    }
}