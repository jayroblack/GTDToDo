using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public ProjectController(IProfileFactory profileFactory,
            ICreateIdsStrategy createIdsStrategy,
            IQueryHandler<GetUserProjectsQuery, GetUserProjectsQueryResult> getProjects,
            IQueryHandler<ProjectQuery, ProjectQueryResult> projectQuery,
            IServiceOptOutcomes<CreateNewUserProjectServiceArg, CreateNewUserProjectServiceResult, CreateUserProjectOutcomes> createNewProjectService)
        {
            _getProjects = getProjects ?? throw new ArgumentNullException(nameof(getProjects));
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            _createIdsStrategy = createIdsStrategy ?? throw new ArgumentNullException(nameof(createIdsStrategy));
            _projectQuery = projectQuery ?? throw new ArgumentNullException(nameof(projectQuery));
            _createNewProjectService = createNewProjectService ?? throw new ArgumentNullException(nameof(createNewProjectService));
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

        [HttpPost]
        [Route("/project")]
        public async Task<IActionResult> Post([FromBody]string projectName)
        {
            var id = _createIdsStrategy.NewId();
            var profile = _profileFactory.GetCurrentProfile();
            var optionResult = 
                await _createNewProjectService.Run(new CreateNewUserProjectServiceArg(id, profile.UserId, projectName));

            return optionResult.Match<IActionResult>(some => Json(some.Project), outcomes => UnprocessableEntity(outcomes.ToString()));
        }

        [HttpPut]
        [Route("/project/{projectId}")]
        public Task<IActionResult> Put([FromRoute]int projectId, [FromBody]MutableProject projectItem)
        {
            //OK - Return fresh copy
            //Does Not Exist
            //Unprocessable Entity
            //Conflict
            throw new NotImplementedException();
        }
    }

    public class MutableProject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int CountOverdue { get; set; }
        public bool IsDeleted { get; set; }
        public int CountOverDue { get; set; }
        public int VersionNumber { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
