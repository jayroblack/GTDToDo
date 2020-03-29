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

    public class UpdateProject : IServiceOpt<UpdateProjectNameArg,
        UpdateProjectNameResult, UpdateProjectOutcome>
    {
        private readonly IQueryHandler<GetProject, GetProjectResult> _getProject;
        private readonly IQueryHandler<GetProjects, GetProjectsResult> _getProjects;
        private readonly ILogger _logger;

        private readonly
            IServiceOpt<PersistUpdateProjectArg, PersistUpdateProjectResult, PersistUpdateProjectOutcome>
            _persistUpdateProject;

        private readonly IProfileFactory _profileFactory;

        public UpdateProject(
            IProfileFactory profileFactory,
            ILogger logger,
            IQueryHandler<GetProject, GetProjectResult> getProject,
            IServiceOpt<PersistUpdateProjectArg, PersistUpdateProjectResult, PersistUpdateProjectOutcome>
                persistUpdateProject,
            IQueryHandler<GetProjects, GetProjectsResult> getProjects)
        {
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getProject = getProject ?? throw new ArgumentNullException(nameof(getProject));
            _persistUpdateProject =
                persistUpdateProject ?? throw new ArgumentNullException(nameof(persistUpdateProject));
            _getProjects = getProjects ?? throw new ArgumentNullException(nameof(getProjects));
        }

        public async Task<Option<UpdateProjectNameResult, UpdateProjectOutcome>> Run(UpdateProjectNameArg nameArg)
        {
            var projectOption = Option.None<GetProjectResult>();

            try
            {
                projectOption = await _getProject.Run(new GetProject(nameArg.Id));
                if (!projectOption.HasValue)
                    return Option.None<UpdateProjectNameResult, UpdateProjectOutcome>(UpdateProjectOutcome
                        .DoesNotExist);
            }
            catch (ArgumentException e) //<== Why did we add this again?
            {
                _logger.Log(LogLevel.Error, e.Message);
                return Option.None<UpdateProjectNameResult, UpdateProjectOutcome>(UpdateProjectOutcome
                    .UnprocessableEntity);
            }

            Project project = null;
            projectOption.MatchSome(some =>
            {
                var proj = some.Project;
                project = new Project(proj.Id, proj.Name, proj.UserId, proj.Count, proj.CountOverDue, proj.DateCreated,
                    proj.VersionNumber, proj.IsDeleted);
            });

            var profile = _profileFactory.GetCurrentProfile();
            if (project.UserId != profile.UserId)
                return Option.None<UpdateProjectNameResult, UpdateProjectOutcome>(UpdateProjectOutcome
                    .NotAuthorized);

            if (project.Name != nameArg.Name)
            {
                var projectsOptional = await _getProjects.Run(new GetProjects(profile.UserId));

                var nameExists = false;
                projectsOptional.MatchSome(some =>
                {
                    nameExists =
                        some.Projects.Any(x => !x.IsDeleted && x.Name == nameArg.Name);
                });

                if (nameExists)
                    return Option.None<UpdateProjectNameResult, UpdateProjectOutcome>(UpdateProjectOutcome
                        .NameAlreadyExists);
            }

            try
            {
                project.SetName(nameArg.Name);
                project.SetVersionNumber(nameArg.VersionNumber);
            }
            catch (ArgumentException e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                return Option.None<UpdateProjectNameResult, UpdateProjectOutcome>(UpdateProjectOutcome
                    .UnprocessableEntity);
            }

            var updatedProjectOption = await _persistUpdateProject.Run(new PersistUpdateProjectArg(project));

            return updatedProjectOption.Match(some =>
                Option.Some<UpdateProjectNameResult, UpdateProjectOutcome>(
                    new UpdateProjectNameResult(some.Project)), outcome =>
                Option.None<UpdateProjectNameResult, UpdateProjectOutcome>(UpdateProjectOutcome
                    .VersionConflict));
        }
    }
}