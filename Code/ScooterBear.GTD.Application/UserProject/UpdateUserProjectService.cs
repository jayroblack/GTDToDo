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

    public class UpdateUserProjectService : IServiceOptOutcomes<UpdateUserProjectServiceArg,
                UpdateUserProjectServiceResult, UpdateProjectOutcome>
    {
        private readonly IProfileFactory _profileFactory;
        private readonly ILogger _logger;
        private readonly IQueryHandler<ProjectQuery, ProjectQueryResult> _getProjectQuery;
        private readonly IServiceOptOutcomes<PersistUpdateProjectServiceArgs, PersistUpdateProjectServiceResult, PersistUpdateProjectOutcome> _persistUpdateProject;
        private readonly IQueryHandler<GetUserProjectsQuery, GetUserProjectsQueryResult> _getProjectsQuery;

        public UpdateUserProjectService(
            IProfileFactory profileFactory,
            ILogger logger,
            IQueryHandler<ProjectQuery, ProjectQueryResult> getProjectQuery,
            IServiceOptOutcomes<PersistUpdateProjectServiceArgs, PersistUpdateProjectServiceResult, PersistUpdateProjectOutcome> persistUpdateProject,
            IQueryHandler<GetUserProjectsQuery, GetUserProjectsQueryResult> getProjectsQuery)
        {
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getProjectQuery = getProjectQuery ?? throw new ArgumentNullException(nameof(getProjectQuery));
            _persistUpdateProject = persistUpdateProject ?? throw new ArgumentNullException(nameof(persistUpdateProject));
            _getProjectsQuery = getProjectsQuery ?? throw new ArgumentNullException(nameof(getProjectsQuery));
        }

        public async Task<Option<UpdateUserProjectServiceResult, UpdateProjectOutcome>> Run(UpdateUserProjectServiceArg arg)
        {
            Option<ProjectQueryResult> userProjectOption = Option.None<ProjectQueryResult>();

            try
            {
                userProjectOption = await _getProjectQuery.Run(new ProjectQuery(arg.ProjectId));
                if (!userProjectOption.HasValue)
                    return Option.None<UpdateUserProjectServiceResult, UpdateProjectOutcome>(UpdateProjectOutcome
                        .DoesNotExist);
            }
            catch (ArgumentException e)//<== Why did we add this again?
            {
                _logger.Log(LogLevel.Error, e.Message);
                return Option.None<UpdateUserProjectServiceResult, UpdateProjectOutcome>(UpdateProjectOutcome
                    .UnprocessableEntity);
            }

            Project project = null;
            userProjectOption.MatchSome(some =>
            {
                var proj = some.UserProject;
                project = new Project(proj.Id, proj.Name, proj.UserId, proj.Count , proj.CountOverDue, proj.DateCreated, proj.VersionNumber, proj.IsDeleted);
            });

            var profile = _profileFactory.GetCurrentProfile();
            if( project.UserId != profile.UserId)
                return Option.None<UpdateUserProjectServiceResult, UpdateProjectOutcome>(UpdateProjectOutcome
                    .NotAuthorized);

            if (project.Name != arg.Name)
            {
                var projectsOptional = await _getProjectsQuery.Run(new GetUserProjectsQuery(profile.UserId));

                bool nameExists = false;
                projectsOptional.MatchSome(some =>
                {
                    nameExists =
                        some.UserProjects.Projects.Any(x => !x.IsDeleted && x.Name == arg.Name);
                });

                if (nameExists)
                    return Option.None<UpdateUserProjectServiceResult, UpdateProjectOutcome>(UpdateProjectOutcome
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
                return Option.None<UpdateUserProjectServiceResult, UpdateProjectOutcome>(UpdateProjectOutcome
                    .UnprocessableEntity);
            }

            var updatedProjectOption = await _persistUpdateProject.Run(new PersistUpdateProjectServiceArgs(project));

            return updatedProjectOption.Match(some =>
                Option.Some<UpdateUserProjectServiceResult, UpdateProjectOutcome>(
                    new UpdateUserProjectServiceResult(some.Project)), outcome =>
                Option.None<UpdateUserProjectServiceResult, UpdateProjectOutcome>(UpdateProjectOutcome
                    .VersionConflict));
        }
    }
}
