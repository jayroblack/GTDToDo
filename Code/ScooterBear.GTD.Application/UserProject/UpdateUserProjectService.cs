using System;
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
        NotAuthorized
    }

    public class UpdateUserProjectService : IServiceOptOutcomes<UpdateUserProjectServiceArg,
        UpdateUserProjectServiceResult, UpdateProjectOutcome>
    {
        private readonly IProfileFactory _profileFactory;
        private readonly ILogger _logger;
        private readonly IQueryHandler<ProjectQuery, ProjectQueryResult> _getProject;
        private readonly IServiceOptOutcomes<PersistUpdateProjectServiceArgs, PersistUpdateProjectServiceResult, PersistUpdateProjectOutcome> _persistUpdateProject;

        public UpdateUserProjectService(
            IProfileFactory profileFactory,
            ILogger logger,
            IQueryHandler<ProjectQuery, ProjectQueryResult> getProject,
            IServiceOptOutcomes<PersistUpdateProjectServiceArgs, PersistUpdateProjectServiceResult, PersistUpdateProjectOutcome> persistUpdateProject)
        {
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getProject = getProject ?? throw new ArgumentNullException(nameof(getProject));
            _persistUpdateProject = persistUpdateProject ?? throw new ArgumentNullException(nameof(persistUpdateProject));
        }

        public async Task<Option<UpdateUserProjectServiceResult, UpdateProjectOutcome>> Run(UpdateUserProjectServiceArg arg)
        {
            var userProjectOption = await _getProject.Run(new ProjectQuery(arg.ProjectId));
            if (!userProjectOption.HasValue)
                return Option.None<UpdateUserProjectServiceResult, UpdateProjectOutcome>(UpdateProjectOutcome
                    .DoesNotExist);

            Project project = null;
            userProjectOption.MatchSome(some =>
            {
                var proj = some.UserProject;
                project = new Project(proj.Id, proj.Name, proj.UserId, proj.Count, proj.IsDeleted, proj.CountOverDue, proj.VersionNumber, proj.DateCreated);
            });

            var profile = _profileFactory.GetCurrentProfile();
            if( project.UserId != profile.UserId)
                return Option.None<UpdateUserProjectServiceResult, UpdateProjectOutcome>(UpdateProjectOutcome
                    .NotAuthorized);

            try
            {
                project.SetCount(arg.Count);
                project.SetCountOverDue(arg.CountOverdue);
                project.SetIsDeleted(arg.IsDeleted);
                project.SetName(arg.Name);
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
