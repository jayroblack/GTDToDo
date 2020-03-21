using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public enum UpdateProjectOutcome
    {
        DoesNotExist,
        VersionConflict,
        UnprocessableEntity,
        NotAuthorized,
        NameAlreadyExists
    }

    public class UpdateProjectService : IServiceOpt<UpdateProjectArg,
        UpdateProjectResult, UpdateProjectOutcome>
    {
        private readonly IQueryHandler<GetProject, GetProjectResult> _getProjectQuery;
        private readonly IQueryHandler<GetProjects, GetProjectsResult> _getProjectsQuery;
        private readonly ILogger _logger;

        private readonly
            IServiceOpt<PersistUpdateProjectArg, PersistUpdateProjectResult, PersistUpdateProjectOutcome>
            _persistUpdateProject;

        private readonly IProfileFactory _profileFactory;

        public UpdateProjectService(
            IProfileFactory profileFactory,
            ILogger logger,
            IQueryHandler<GetProject, GetProjectResult> getProjectQuery,
            IServiceOpt<PersistUpdateProjectArg, PersistUpdateProjectResult, PersistUpdateProjectOutcome>
                persistUpdateProject,
            IQueryHandler<GetProjects, GetProjectsResult> getProjectsQuery)
        {
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getProjectQuery = getProjectQuery ?? throw new ArgumentNullException(nameof(getProjectQuery));
            _persistUpdateProject =
                persistUpdateProject ?? throw new ArgumentNullException(nameof(persistUpdateProject));
            _getProjectsQuery = getProjectsQuery ?? throw new ArgumentNullException(nameof(getProjectsQuery));
        }

        public async Task<Option<UpdateProjectResult, UpdateProjectOutcome>> Run(UpdateProjectArg arg)
        {
            var userProjectOption = Option.None<GetProjectResult>();

            try
            {
                userProjectOption = await _getProjectQuery.Run(new GetProject(arg.ProjectId));
                if (!userProjectOption.HasValue)
                    return Option.None<UpdateProjectResult, UpdateProjectOutcome>(UpdateProjectOutcome
                        .DoesNotExist);
            }
            catch (ArgumentException e) //<== Why did we add this again?
            {
                _logger.Log(LogLevel.Error, e.Message);
                return Option.None<UpdateProjectResult, UpdateProjectOutcome>(UpdateProjectOutcome
                    .UnprocessableEntity);
            }

            Project project = null;
            userProjectOption.MatchSome(some =>
            {
                var proj = some.UserProject;
                project = new Project(proj.Id, proj.Name, proj.UserId, proj.Count, proj.CountOverDue, proj.DateCreated,
                    proj.VersionNumber, proj.IsDeleted);
            });

            var profile = _profileFactory.GetCurrentProfile();
            if (project.UserId != profile.UserId)
                return Option.None<UpdateProjectResult, UpdateProjectOutcome>(UpdateProjectOutcome
                    .NotAuthorized);

            if (project.Name != arg.Name)
            {
                var projectsOptional = await _getProjectsQuery.Run(new GetProjects(profile.UserId));

                var nameExists = false;
                projectsOptional.MatchSome(some =>
                {
                    nameExists =
                        some.Projects.Any(x => !x.IsDeleted && x.Name == arg.Name);
                });

                if (nameExists)
                    return Option.None<UpdateProjectResult, UpdateProjectOutcome>(UpdateProjectOutcome
                        .NameAlreadyExists);
            }

            try
            {
                project.SetCount(arg.Count);
                project.SetCountOverDue(arg.CountOverdue);
                project.SetIsDeleted(arg.IsDeleted);
                project.SetName(arg.Name);
                project.SetVersionNumber(arg.VersionNumber);
            }
            catch (ArgumentException e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                return Option.None<UpdateProjectResult, UpdateProjectOutcome>(UpdateProjectOutcome
                    .UnprocessableEntity);
            }

            var updatedProjectOption = await _persistUpdateProject.Run(new PersistUpdateProjectArg(project));

            return updatedProjectOption.Match(some =>
                Option.Some<UpdateProjectResult, UpdateProjectOutcome>(
                    new UpdateProjectResult(some.Project)), outcome =>
                Option.None<UpdateProjectResult, UpdateProjectOutcome>(UpdateProjectOutcome
                    .VersionConflict));
        }
    }
}