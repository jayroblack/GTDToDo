using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ICreateIdsStrategy _createIdsStrategy;

        private readonly IServiceOptOutcomes<CreateNewProjectArg, CreateNewProjectResult, CreateUserProjectOutcomes>
            _createNewProjectService;

        private readonly IServiceOptOutcomes<DeleteProjectArgs, DeleteProjectResult, DeleteUserProjectOutcome>
            _deleteProjectService;

        private readonly IQueryHandler<GetProjects, GetProjectsResult> _getProjects;
        private readonly IProfileFactory _profileFactory;
        private readonly IQueryHandler<GetProject, GetProjectResult> _projectQuery;

        private readonly IServiceOptOutcomes<UpdateProjectArg, UpdateProjectResult, UpdateProjectOutcome>
            _updatProjectService;

        public ProjectController(IProfileFactory profileFactory,
            ICreateIdsStrategy createIdsStrategy,
            IQueryHandler<GetProjects, GetProjectsResult> getProjects,
            IQueryHandler<GetProject, GetProjectResult> projectQuery,
            IServiceOptOutcomes<CreateNewProjectArg, CreateNewProjectResult, CreateUserProjectOutcomes>
                createNewProjectService,
            IServiceOptOutcomes<UpdateProjectArg,
                UpdateProjectResult, UpdateProjectOutcome> updateProjectService,
            IServiceOptOutcomes<DeleteProjectArgs, DeleteProjectResult, DeleteUserProjectOutcome> deleteProjectService)
        {
            _getProjects = getProjects ?? throw new ArgumentNullException(nameof(getProjects));
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            _createIdsStrategy = createIdsStrategy ?? throw new ArgumentNullException(nameof(createIdsStrategy));
            _projectQuery = projectQuery ?? throw new ArgumentNullException(nameof(projectQuery));
            _createNewProjectService = createNewProjectService ??
                                       throw new ArgumentNullException(nameof(createNewProjectService));
            _updatProjectService =
                updateProjectService ?? throw new ArgumentNullException(nameof(updateProjectService));
            _deleteProjectService =
                deleteProjectService ?? throw new ArgumentNullException(nameof(deleteProjectService));
        }

        [HttpGet]
        [Route("/projects")]
        public async Task<IActionResult> Get()
        {
            var profile = _profileFactory.GetCurrentProfile();
            var projectResultOption = await _getProjects.Run(new GetProjects(profile.UserId));
            return projectResultOption.Match(some => Json(some),
                () => Json(new List<IProject>()));
        }

        [HttpGet]
        [Route("/project/{projectId}")]
        public async Task<IActionResult> Get([FromRoute] string projectId)
        {
            var resultOption =
                await _projectQuery.Run(new GetProject(projectId));

            return resultOption.Match<IActionResult>(some => Json(some.UserProject),
                NotFound);
        }

        [HttpPost]
        [Route("/project")]
        public async Task<IActionResult> Post([FromBody] NewProjectValues data)
        {
            var id = _createIdsStrategy.NewId();
            var profile = _profileFactory.GetCurrentProfile();
            var optionResult =
                await _createNewProjectService.Run(new CreateNewProjectArg(id, data.NewProjectName));

            return optionResult.Match<IActionResult>(some => Json(some.Project),
                outcomes => UnprocessableEntity(outcomes.ToString()));
        }

        [HttpPut]
        [Route("/project/{projectId}")]
        public async Task<IActionResult> Put([FromRoute] string projectId, [FromBody] MutableProject projectItem)
        {
            var optionResult = await
                _updatProjectService.Run(new UpdateProjectArg(projectId, projectItem.Name, projectItem.Count,
                    projectItem.IsDeleted, projectItem.CountOverdue, projectItem.VersionNumber));

            return optionResult.Match<IActionResult>(
                some => Ok(some),
                outcome =>
                {
                    if (outcome == UpdateProjectOutcome.NotAuthorized)
                        return Unauthorized();

                    if (outcome == UpdateProjectOutcome.DoesNotExist)
                        return NotFound();

                    if (outcome == UpdateProjectOutcome.UnprocessableEntity)
                        return UnprocessableEntity(outcome.ToString());

                    return Conflict(outcome);
                });
        }

        [HttpDelete]
        [Route("/project/{projectId}")]
        public async Task<IActionResult> Delete([FromRoute] string projectId)
        {
            var optionResult = await
                _deleteProjectService.Run(new DeleteProjectArgs(projectId));

            return optionResult.Match<IActionResult>(
                some => Ok(),
                outcome =>
                {
                    if (outcome == DeleteUserProjectOutcome.NotAuthorized)
                        return Unauthorized();

                    if (outcome == DeleteUserProjectOutcome.NotFound)
                        return NotFound();

                    if (outcome == DeleteUserProjectOutcome.HasAssociatedToDos)
                        return UnprocessableEntity("Has associated To Do Items.");

                    return Conflict(outcome);
                });
        }

        public class NewProjectValues
        {
            [JsonProperty("projectName")] public string NewProjectName { get; set; }
        }
    }

    public class MutableProject
    {
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("count")] public int Count { get; set; }
        [JsonProperty("countOverdue")] public int CountOverdue { get; set; }
        [JsonProperty("isDeleted")] public bool IsDeleted { get; set; }
        [JsonProperty("versionNumber")] public int VersionNumber { get; set; }
    }
}