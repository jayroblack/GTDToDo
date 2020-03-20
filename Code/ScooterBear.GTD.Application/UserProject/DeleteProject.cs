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
        HasAssociatedToDos, //TODO: Once you are creating ToDos - check that it is not in use.
        VersionConflict
    }

    public class DeleteProject : IServiceOptOutcomes<DeleteProjectArgs, DeleteProjectResult, DeleteUserProjectOutcome>
    {
        private readonly IQueryHandler<GetProject, GetProjectResult> _getProject;
        private readonly ILogger _logger;

        private readonly
            IServiceOptOutcomes<PersistUpdateProjectArgs, PersistUpdateProjectResult, PersistUpdateProjectOutcome>
            _persistUpdateProject;

        private readonly IProfileFactory _profileFactory;

        public DeleteProject(
            IProfileFactory profileFactory,
            ILogger logger,
            IQueryHandler<GetProject, GetProjectResult> getProject,
            IServiceOptOutcomes<PersistUpdateProjectArgs, PersistUpdateProjectResult, PersistUpdateProjectOutcome>
                persistUpdateProject)
        {
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getProject = getProject;
            _persistUpdateProject =
                persistUpdateProject ?? throw new ArgumentNullException(nameof(persistUpdateProject));
        }

        public async Task<Option<DeleteProjectResult, DeleteUserProjectOutcome>> Run(DeleteProjectArgs arg)
        {
            var userProjectOption = await _getProject.Run(new GetProject(arg.ProjectId));
            if (!userProjectOption.HasValue)
                return Option.None<DeleteProjectResult, DeleteUserProjectOutcome>(DeleteUserProjectOutcome
                    .NotFound);

            var profile = _profileFactory.GetCurrentProfile();
            Project project = null;
            userProjectOption.MatchSome(some =>
            {
                var proj = some.UserProject;
                project = new Project(proj.Id, proj.Name, proj.UserId, proj.Count, proj.CountOverDue, proj.DateCreated,
                    proj.VersionNumber, proj.IsDeleted);
            });

            if (profile.UserId != project.UserId)
                return Option.None<DeleteProjectResult, DeleteUserProjectOutcome>(DeleteUserProjectOutcome
                    .NotAuthorized);

            project.SetIsDeleted(true);

            var updatedProjectOption = await _persistUpdateProject.Run(new PersistUpdateProjectArgs(project));

            return updatedProjectOption.Match(some =>
                Option.Some<DeleteProjectResult, DeleteUserProjectOutcome>(
                    new DeleteProjectResult()), outcome =>
                Option.None<DeleteProjectResult, DeleteUserProjectOutcome>(DeleteUserProjectOutcome.VersionConflict));
        }
    }
}