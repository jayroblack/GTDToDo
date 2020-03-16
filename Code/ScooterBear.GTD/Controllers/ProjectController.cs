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
        private readonly IQueryHandler<GetUserProjectsQuery, GetUserProjectsQueryResult> _getProjects;
        private readonly IProfileFactory _profileFactory;
        private readonly ICreateIdsStrategy _createIdsStrategy;
        private readonly IQueryHandler<ProjectQuery, ProjectQueryResult> _projectQuery;
        private readonly IServiceOptOutcomes<CreateNewUserProjectServiceArg, CreateNewUserProjectServiceResult, CreateUserProjectOutcomes> _createNewProjectService;
        private readonly IServiceOptOutcomes<UpdateUserProjectServiceArg, UpdateUserProjectServiceResult, UpdateProjectOutcome> _updatProjectService;

        public ProjectController(IProfileFactory profileFactory,
            ICreateIdsStrategy createIdsStrategy,
            IQueryHandler<GetUserProjectsQuery, GetUserProjectsQueryResult> getProjects,
            IQueryHandler<ProjectQuery, ProjectQueryResult> projectQuery,
            IServiceOptOutcomes<CreateNewUserProjectServiceArg, CreateNewUserProjectServiceResult, CreateUserProjectOutcomes> createNewProjectService,
            IServiceOptOutcomes<UpdateUserProjectServiceArg,
                UpdateUserProjectServiceResult, UpdateProjectOutcome> updatProjectService)
        {
            _getProjects = getProjects ?? throw new ArgumentNullException(nameof(getProjects));
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            _createIdsStrategy = createIdsStrategy ?? throw new ArgumentNullException(nameof(createIdsStrategy));
            _projectQuery = projectQuery ?? throw new ArgumentNullException(nameof(projectQuery));
            _createNewProjectService = createNewProjectService ?? throw new ArgumentNullException(nameof(createNewProjectService));
            _updatProjectService = updatProjectService ?? throw new ArgumentNullException(nameof(updatProjectService));
        }

        [HttpGet]
        [Route("/projects")]
        public async Task<IActionResult> Get()
        {
            var profile = _profileFactory.GetCurrentProfile();
            var projectResultOption = await _getProjects.Run(new GetUserProjectsQuery(profile.UserId));
            return projectResultOption.Match(some => Json(some.UserProjects),
                () => Json(new List<IProject>()));
        }

        [HttpGet]
        [Route("/project/{projectId}")]
        public async Task<IActionResult> Get([FromRoute]string projectId)
        {
            var resultOption =
                await _projectQuery.Run(new ProjectQuery(projectId));

            return resultOption.Match<IActionResult>(some => Json(some.UserProject), 
                NotFound);
        }

        public class NewProjectValues
        {
            [JsonProperty("projectName")]
            public string NewProjectName { get; set; }  
        }

        [HttpPost]
        [Route("/project")]
        public async Task<IActionResult> Post([FromBody]NewProjectValues data)
        {
            var id = _createIdsStrategy.NewId();
            var profile = _profileFactory.GetCurrentProfile();
            var optionResult = 
                await _createNewProjectService.Run(new CreateNewUserProjectServiceArg(id, profile.UserId, data.NewProjectName));

            return optionResult.Match<IActionResult>(some => Json(some.Project), outcomes => UnprocessableEntity(outcomes.ToString()));
        }

        [HttpPut]
        [Route("/project/{projectId}")]
        public async Task<IActionResult> Put([FromRoute] string projectId, [FromBody] MutableProject projectItem)
        {
            var optionResult = await
                _updatProjectService.Run(new UpdateUserProjectServiceArg(projectId, projectItem.Name, projectItem.Count,
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
        public async Task<IActionResult> Delete([FromRoute]string projectId)
        {

        }
    }

    public class MutableProject
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("countOverdue")]
        public int CountOverdue { get; set; }
        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }
        [JsonProperty("versionNumber")]
        public int VersionNumber { get; set; }
    }
}
