using System;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserProject
{
    public enum DeleteProjectOutcome
    {
        NotAuthorized,
        NotFound,
        HasAssociatedToDos, //TODO: Once you are creating ToDos - check that it is not in use.
        VersionConflict
    }

    public class DeleteProject : IServiceOpt<DeleteProjectArg, DeleteProjectResult, DeleteProjectOutcome>
    {
        private readonly IQueryHandler<GetProject, GetProjectResult> _getProject;
        private readonly
            IServiceOpt<PersistUpdateProjectArg, PersistUpdateProjectResult, PersistUpdateProjectOutcome>
            _persistUpdateProject;

        private readonly IProfileFactory _profileFactory;

        public DeleteProject(
            IProfileFactory profileFactory,
            IQueryHandler<GetProject, GetProjectResult> getProject,
            IServiceOpt<PersistUpdateProjectArg, PersistUpdateProjectResult, PersistUpdateProjectOutcome>
                persistUpdateProject)
        {
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            _getProject = getProject;
            _persistUpdateProject =
                persistUpdateProject ?? throw new ArgumentNullException(nameof(persistUpdateProject));
        }

        public async Task<Option<DeleteProjectResult, DeleteProjectOutcome>> Run(DeleteProjectArg arg)
        {
            var userProjectOption = await _getProject.Run(new GetProject(arg.ProjectId));
            if (!userProjectOption.HasValue)
                return Option.None<DeleteProjectResult, DeleteProjectOutcome>(DeleteProjectOutcome
                    .NotFound);

            var profile = _profileFactory.GetCurrentProfile();
            Project project = null;
            userProjectOption.MatchSome(some =>
            {
                var proj = some.Project;
                project = new Project(proj.Id, proj.Name, proj.UserId, proj.Count, proj.CountOverDue, proj.DateCreated,
                    proj.VersionNumber, proj.IsDeleted);
            });

            if (profile.UserId != project.UserId)
                return Option.None<DeleteProjectResult, DeleteProjectOutcome>(DeleteProjectOutcome
                    .NotAuthorized);

            project.SetIsDeleted(true);

            var updatedProjectOption = await _persistUpdateProject.Run(new PersistUpdateProjectArg(project));

            return updatedProjectOption.Match(some =>
                Option.Some<DeleteProjectResult, DeleteProjectOutcome>(
                    new DeleteProjectResult()), outcome =>
                Option.None<DeleteProjectResult, DeleteProjectOutcome>(DeleteProjectOutcome.VersionConflict));
        }
    }
}