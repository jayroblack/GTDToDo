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
        private readonly IServiceOpt<CreateNewProjectArg, CreateNewProjectResult, CreateUserProjectOutcomes> _createNewProject;
        private readonly IServiceOpt<DeleteProjectArg, DeleteProjectResult, DeleteUserProjectOutcome> _deleteProject;
        private readonly IQueryHandler<GetProjects, GetProjectsResult> _getProjects;
        private readonly IProfileFactory _profileFactory;
        private readonly IQueryHandler<GetProject, GetProjectResult> _projectQuery;
        private readonly IServiceOpt<UpdateProjectNameArg, UpdateProjectNameResult, UpdateProjectOutcome> _updateProject;

        public ProjectController(IProfileFactory profileFactory,
            ICreateIdsStrategy createIdsStrategy,
            IQueryHandler<GetProjects, GetProjectsResult> getProjects,
            IQueryHandler<GetProject, GetProjectResult> projectQuery,
            IServiceOpt<CreateNewProjectArg, CreateNewProjectResult, CreateUserProjectOutcomes> createNewProject,
            IServiceOpt<UpdateProjectNameArg, UpdateProjectNameResult, UpdateProjectOutcome> updateProject,
            IServiceOpt<DeleteProjectArg, DeleteProjectResult, DeleteUserProjectOutcome> deleteProject)
        {
            _getProjects = getProjects ?? throw new ArgumentNullException(nameof(getProjects));
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            _createIdsStrategy = createIdsStrategy ?? throw new ArgumentNullException(nameof(createIdsStrategy));
            _projectQuery = projectQuery ?? throw new ArgumentNullException(nameof(projectQuery));
            _createNewProject = createNewProject ?? throw new ArgumentNullException(nameof(createNewProject));
            _updateProject = updateProject ?? throw new ArgumentNullException(nameof(updateProject));
            _deleteProject = deleteProject ?? throw new ArgumentNullException(nameof(deleteProject));
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
        public async Task<IActionResult> Post([FromBody] ProjectValues data)
        {
            var id = _createIdsStrategy.NewId();
            var profile = _profileFactory.GetCurrentProfile();
            var optionResult =
                await _createNewProject.Run(new CreateNewProjectArg(id, data.Name));

            return optionResult.Match<IActionResult>(some => Json(some.Project),
                outcomes => UnprocessableEntity(outcomes.ToString()));
        }

        [HttpPut]
        [Route("/project/{projectId}")]
        public async Task<IActionResult> Put([FromRoute] string projectId, [FromBody] ProjectValues projectItem)
        {
            var optionResult = await
                _updateProject.Run(new UpdateProjectNameArg(projectId, projectItem.Name, projectItem.VersionNumber));

            return optionResult.Match<IActionResult>(
                some => Ok(some.Project),
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
                _deleteProject.Run(new DeleteProjectArg(projectId));

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

        public class ProjectValues
        {
            [JsonProperty("name")] public string Name { get; set; }
            [JsonProperty("versionNumber")] public int VersionNumber { get; set; }
        }
    }
}