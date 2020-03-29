using System;
using System.Threading.Tasks;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserLabel
{
    public enum DeleteLabelOutcome
    {
        NotAuthorized,
        NotFound,
        HasAssociatedToDos, //TODO: Once you are creating ToDos - check that it is not in use.
        VersionConflict
    }

    public class DeleteLabel : IServiceOpt<DeleteLabelArg, DeleteLabelResult, DeleteLabelOutcome>
    {
        private readonly IProfileFactory _profileFactory;
        private readonly IQueryHandler<GetLabel, GetLabelResult> _getLabel;

        private readonly IServiceOpt<PersistUpdateLabelArg, PersistUpdateLabelResult, PersistUpdateLabelOutcome>
            _persistUpdateLabel;

        public DeleteLabel(
            IProfileFactory profileFactory,
            IQueryHandler<GetLabel, GetLabelResult> getLabel,
            IServiceOpt<PersistUpdateLabelArg, PersistUpdateLabelResult, PersistUpdateLabelOutcome> persistUpdateLabel)
        {
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            _getLabel = getLabel ?? throw new ArgumentNullException(nameof(getLabel));
            _persistUpdateLabel = persistUpdateLabel ?? throw new ArgumentNullException(nameof(persistUpdateLabel));
        }

        public async Task<Option<DeleteLabelResult, DeleteLabelOutcome>> Run(DeleteLabelArg arg)
        {
            var labelOption = await _getLabel.Run(new GetLabel(arg.LabelId));
            if (!labelOption.HasValue)
                return Option.None<DeleteLabelResult, DeleteLabelOutcome>(DeleteLabelOutcome
                    .NotFound);

            var profile = _profileFactory.GetCurrentProfile();
            Label label = null;
            labelOption.MatchSome(some =>
            {
                var proj = some.Label;
                label = new Label(proj.Id, proj.Name, proj.UserId, proj.Count, proj.CountOverDue, proj.DateCreated,
                    proj.VersionNumber, proj.IsDeleted);
            });

            if (profile.UserId != label.UserId)
                return Option.None<DeleteLabelResult, DeleteLabelOutcome>(DeleteLabelOutcome
                    .NotAuthorized);

            label.SetIsDeleted(true);

            var updateLabelOption = await _persistUpdateLabel.Run(new PersistUpdateLabelArg(label));

            return updateLabelOption.Match(some =>
                Option.Some<DeleteLabelResult, DeleteLabelOutcome>(
                    new DeleteLabelResult()), outcome =>
                Option.None<DeleteLabelResult, DeleteLabelOutcome>(DeleteLabelOutcome.VersionConflict));
        }
    }
}