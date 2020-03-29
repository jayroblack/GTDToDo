using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Optional;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserLabel
{
    public enum UpdateLabelOutcome
    {
        DoesNotExist,
        VersionConflict,
        UnprocessableEntity,
        NotAuthorized,
        NameAlreadyExists
    }

    public class UpdateLabel : IServiceOpt<UpdateLabelNameArg,
        UpdateLabelNameResult, UpdateLabelOutcome>
    {
        private readonly ILogger _logger;
        private readonly IQueryHandler<GetLabel, GetLabelResult> _getLabel;
        private readonly IQueryHandler<GetLabels, GetLabelsResult> _getLabels;
        private readonly IServiceOpt<PersistUpdateLabelArg, PersistUpdateLabelResult, PersistUpdateLabelOutcome> _persistUpdateLabel;
        private readonly IProfileFactory _profileFactory;

        public UpdateLabel(
            IProfileFactory profileFactory,
            ILogger logger,
            IQueryHandler<GetLabel, GetLabelResult> getLabel,
            IQueryHandler<GetLabels, GetLabelsResult> getLabels,
            IServiceOpt<PersistUpdateLabelArg, PersistUpdateLabelResult, PersistUpdateLabelOutcome> _persistUpdateLabel
            )
        {
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _getLabel = getLabel ?? throw new ArgumentNullException(nameof(getLabel));
            _getLabels = getLabels ?? throw new ArgumentNullException(nameof(getLabels));
            _persistUpdateLabel = _persistUpdateLabel ?? throw new ArgumentNullException(nameof(_persistUpdateLabel));
        }

        public async Task<Option<UpdateLabelNameResult, UpdateLabelOutcome>> Run(UpdateLabelNameArg nameArg)
        {
            var getLabelOption = Option.None<GetLabelResult>();

            try
            {
                getLabelOption = await _getLabel.Run(new GetLabel(nameArg.Id));
                if (!getLabelOption.HasValue)
                    return Option.None<UpdateLabelNameResult, UpdateLabelOutcome>(UpdateLabelOutcome
                        .DoesNotExist);
            }
            catch (ArgumentException e) //<== Why did we add this again?
            {
                _logger.Log(LogLevel.Error, e.Message);
                return Option.None<UpdateLabelNameResult, UpdateLabelOutcome>(UpdateLabelOutcome
                    .UnprocessableEntity);
            }

            Label labelCandidate = null;
            getLabelOption.MatchSome(some =>
            {
                var label = some.Label;
                labelCandidate = new Label(label.Id, label.Name, label.UserId, label.Count, label.CountOverDue, label.DateCreated,
                    label.VersionNumber, label.IsDeleted);
            });

            var profile = _profileFactory.GetCurrentProfile();
            if (labelCandidate.UserId != profile.UserId)
                return Option.None<UpdateLabelNameResult, UpdateLabelOutcome>(UpdateLabelOutcome
                    .NotAuthorized);

            if (labelCandidate.Name != nameArg.Name)
            {
                var projectsOptional = await _getLabels.Run(new GetLabels(profile.UserId));

                var nameExists = false;
                projectsOptional.MatchSome(some =>
                {
                    nameExists =
                        some.Labels.Any(x => !x.IsDeleted && x.Name == nameArg.Name);
                });

                if (nameExists)
                    return Option.None<UpdateLabelNameResult, UpdateLabelOutcome>(UpdateLabelOutcome
                        .NameAlreadyExists);
            }

            try
            {
                labelCandidate.SetName(nameArg.Name);
                labelCandidate.SetVersionNumber(nameArg.VersionNumber);
            }
            catch (ArgumentException e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                return Option.None<UpdateLabelNameResult, UpdateLabelOutcome>(UpdateLabelOutcome
                    .UnprocessableEntity);
            }

            var updatedProjectOption = await _persistUpdateLabel.Run(new PersistUpdateLabelArg(labelCandidate));

            return updatedProjectOption.Match(some =>
                Option.Some<UpdateLabelNameResult, UpdateLabelOutcome>(
                    new UpdateLabelNameResult(some.Label)), outcome =>
                Option.None<UpdateLabelNameResult, UpdateLabelOutcome>(UpdateLabelOutcome
                    .VersionConflict));
        }
    }
}
