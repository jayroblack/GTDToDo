using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public enum DeleteUserProjectOutcome
    {
        NotAuthorized,
        NotFound,
        HasAssociatedToDos,//TODO: Once you are creating ToDos - check that it is not in use.
        VersionConflict
    }

    public class DeleteUserProjectService : IServiceOptOutcomes<DeleteUserProjectServiceArgs, DeleteUserProjectServiceResult, DeleteUserProjectOutcome>
    {
        private readonly IProfileFactory _profileFactory;
        private readonly ILogger _logger;
        private readonly IQueryHandler<ProjectQuery, ProjectQueryResult> _getProject;
        private readonly IServiceOptOutcomes<PersistUpdateProjectServiceArgs, PersistUpdateProjectServiceResult, PersistUpdateProjectOutcome> _persistUpdateProject;

        public DeleteUserProjectService(
            IProfileFactory profileFactory,
            ILogger logger,
            IQueryHandler<ProjectQuery, ProjectQueryResult> getProject,
            IServiceOptOutcomes<PersistUpdateProjectServiceArgs, PersistUpdateProjectServiceResult, PersistUpdateProjectOutcome> persistUpdateProject)
        {
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getProject = getProject;
            _persistUpdateProject = persistUpdateProject ?? throw new ArgumentNullException(nameof(persistUpdateProject));
        }

        public async Task<Option<DeleteUserProjectServiceResult, DeleteUserProjectOutcome>> Run(DeleteUserProjectServiceArgs arg)
        {
            var userProjectOption = await _getProject.Run(new ProjectQuery(arg.ProjectId));
            if (!userProjectOption.HasValue)
                return Option.None<DeleteUserProjectServiceResult, DeleteUserProjectOutcome>(DeleteUserProjectOutcome
                    .NotFound);

            var profile = _profileFactory.GetCurrentProfile();
            Project project = null;
            userProjectOption.MatchSome(some =>
            {
                var proj = some.UserProject;
                project = new Project(proj.Id, proj.Name, proj.UserId, proj.Count, proj.CountOverDue, proj.DateCreated, proj.VersionNumber, proj.IsDeleted);
            });

            if( profile.UserId != project.UserId)
                return Option.None<DeleteUserProjectServiceResult, DeleteUserProjectOutcome>(DeleteUserProjectOutcome
                    .NotAuthorized);

            project.SetIsDeleted(true);

            var updatedProjectOption = await _persistUpdateProject.Run(new PersistUpdateProjectServiceArgs(project));

            return updatedProjectOption.Match(some =>
                Option.Some<DeleteUserProjectServiceResult, DeleteUserProjectOutcome>(
                    new DeleteUserProjectServiceResult()), outcome =>
                Option.None<DeleteUserProjectServiceResult, DeleteUserProjectOutcome>(DeleteUserProjectOutcome.VersionConflict));
        }
    }
}
