using System;
using System.Linq;
using System.Threading.Tasks;
using Optional;
using Optional.Async.Extensions;
using ScooterBear.GTD.Application.Services.Persistence;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Application.UserLabel
{
    public enum CreateLabelOutcomes
    {
        LabelNameAlreadyExists,
        LabelIdAlreadyExists
    }

    public class CreateNewLabel : IServiceOpt<CreateNewLabelArg, CreateNewLabelResult,
        CreateLabelOutcomes>
    {
        private readonly IQueryHandler<GetLabels, GetLabelsResult> _getLabels;
        private readonly IService<PersistLabelArg, PersistNewLabelResult> _persistNewLabel;
        private readonly IKnowTheDate _iKnowTheDate;
        private readonly IProfileFactory _profileFactory;

        public CreateNewLabel(
            IQueryHandler<GetLabels, GetLabelsResult> getLabels,
            IService<PersistLabelArg, PersistNewLabelResult> persistNewLabel,
            IKnowTheDate iKnowTheDate,
            IProfileFactory profileFactory)
        {
            _getLabels = getLabels ?? throw new ArgumentNullException(nameof(getLabels));
            _persistNewLabel = persistNewLabel ?? throw new ArgumentNullException(nameof(persistNewLabel));
            _iKnowTheDate = iKnowTheDate ?? throw new ArgumentNullException(nameof(iKnowTheDate));
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
        }

        public async Task<Option<CreateNewLabelResult, CreateLabelOutcomes>> Run(CreateNewLabelArg arg)
        {
            var userId = _profileFactory.GetCurrentProfile().UserId;

            var labels= await _getLabels.Run(new GetLabels(userId));

            var dateTimeCreated = _iKnowTheDate.UtcNow();

            return await
                labels.MatchAsync(async some =>
                    {
                        if (some.Labels.Any(x => x.Name == arg.Name && !x.IsDeleted))
                            return Option.None<CreateNewLabelResult, CreateLabelOutcomes>(
                                CreateLabelOutcomes.LabelNameAlreadyExists);

                        if (some.Labels.Any(x => x.Id == arg.Id))
                            return Option.None<CreateNewLabelResult, CreateLabelOutcomes>(
                                CreateLabelOutcomes.LabelIdAlreadyExists);

                        var persistResult = await
                            _persistNewLabel.Run(new PersistLabelArg(arg.Id, userId, arg.Name,
                                dateTimeCreated));

                        return Option.Some<CreateNewLabelResult, CreateLabelOutcomes>(
                            new CreateNewLabelResult(persistResult.Label));
                    },
                    async () =>
                    {
                        var persistResult = await
                            _persistNewLabel.Run(new PersistLabelArg(arg.Id, userId, arg.Name,
                                dateTimeCreated, arg.ConsistentRead));

                        return Option.Some<CreateNewLabelResult, CreateLabelOutcomes>(
                            new CreateNewLabelResult(persistResult.Label));
                    });
        }
    }
}
