using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScooterBear.GTD.Application.UserLabel;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Controllers
{
    [Authorize]
    public class LabelController : Controller
    {
        private readonly ICreateIdsStrategy _createIdsStrategy;
        private readonly IServiceOpt<CreateNewLabelArg, CreateNewLabelResult, CreateLabelOutcomes> _createNewLabel;
        private readonly IServiceOpt<DeleteLabelArg, DeleteLabelResult, DeleteLabelOutcome> _deleteLabel;
        private readonly IQueryHandler<GetLabels, GetLabelsResult> _getLabels;
        private readonly IProfileFactory _profileFactory;
        private readonly IQueryHandler<GetLabel, GetLabelResult> _getLabel;
        private readonly IServiceOpt<UpdateLabelNameArg, UpdateLabelNameResult, UpdateLabelOutcome> _updateLabel;

        public LabelController(IProfileFactory profileFactory,
            ICreateIdsStrategy createIdsStrategy,
            IQueryHandler<GetLabels, GetLabelsResult> getLabels,
            IQueryHandler<GetLabel, GetLabelResult> getLabel,
            IServiceOpt<CreateNewLabelArg, CreateNewLabelResult, CreateLabelOutcomes> createNewLabel,
            IServiceOpt<UpdateLabelNameArg, UpdateLabelNameResult, UpdateLabelOutcome> updateLabel,
            IServiceOpt<DeleteLabelArg, DeleteLabelResult, DeleteLabelOutcome> deleteLabel)
        {
            _getLabels = getLabels ?? throw new ArgumentNullException(nameof(getLabels));
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            _createIdsStrategy = createIdsStrategy ?? throw new ArgumentNullException(nameof(createIdsStrategy));
            _getLabel = getLabel ?? throw new ArgumentNullException(nameof(getLabel));
            _createNewLabel = createNewLabel ?? throw new ArgumentNullException(nameof(createNewLabel));
            _updateLabel = updateLabel ?? throw new ArgumentNullException(nameof(updateLabel));
            _deleteLabel = deleteLabel ?? throw new ArgumentNullException(nameof(deleteLabel));
        }

        [HttpGet]
        [Route("/labels")]
        public async Task<IActionResult> Get()
        {
            var profile = _profileFactory.GetCurrentProfile();
            var getLabelOption = await _getLabels.Run(new GetLabels(profile.UserId));
            return getLabelOption.Match(some => Json(some),
                () => Json(new GetLabelsResult(profile.UserId, new List<ILabel>())));
        }

        [HttpGet]
        [Route("/label/{labelId}")]
        public async Task<IActionResult> Get([FromRoute] string labelId)
        {
            var resultOption =
                await _getLabel.Run(new GetLabel(labelId));

            return resultOption.Match<IActionResult>(some => Json(some.Label),
                NotFound);
        }

        [HttpPost]
        [Route("/label")]
        public async Task<IActionResult> Post([FromBody] LabelController.LabelValues data)
        {
            var id = _createIdsStrategy.NewId();
            var profile = _profileFactory.GetCurrentProfile();
            var optionResult =
                await _createNewLabel.Run(new CreateNewLabelArg(id, data.Name));

            return optionResult.Match<IActionResult>(some => Json(some.Label),
                outcomes => UnprocessableEntity(outcomes.ToString()));
        }

        [HttpPut]
        [Route("/label/{labelId}")]
        public async Task<IActionResult> Put([FromRoute] string labelId, [FromBody] LabelController.LabelValues labelValues)
        {
            var optionResult = await
                _updateLabel.Run(new UpdateLabelNameArg(labelId, labelValues.Name, labelValues.VersionNumber));

            return optionResult.Match<IActionResult>(
                some => Ok(some.Label),
                outcome =>
                {
                    if (outcome == UpdateLabelOutcome.NotAuthorized)
                        return Unauthorized();

                    if (outcome == UpdateLabelOutcome.DoesNotExist)
                        return NotFound();

                    if (outcome == UpdateLabelOutcome.UnprocessableEntity)
                        return UnprocessableEntity(outcome.ToString());

                    return Conflict(outcome);
                });
        }

        [HttpDelete]
        [Route("/label/{labelId}")]
        public async Task<IActionResult> Delete([FromRoute] string labelId)
        {
            var optionResult = await
                _deleteLabel.Run(new DeleteLabelArg(labelId));

            return optionResult.Match<IActionResult>(
                some => Ok(),
                outcome =>
                {
                    if (outcome == DeleteLabelOutcome.NotAuthorized)
                        return Unauthorized();

                    if (outcome == DeleteLabelOutcome.NotFound)
                        return NotFound();

                    if (outcome == DeleteLabelOutcome.HasAssociatedToDos)
                        return UnprocessableEntity("Has associated To Do Items.");

                    return Conflict(outcome);
                });
        }

        public class LabelValues
        {
            [JsonProperty("name")] public string Name { get; set; }
            [JsonProperty("versionNumber")] public int VersionNumber { get; set; }
        }
    }
}
