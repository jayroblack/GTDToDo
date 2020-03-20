using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScooterBear.GTD.Application.UserLabel;
using ScooterBear.GTD.Application.UserProfile;
using ScooterBear.GTD.Application.UserProject;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private readonly IQueryHandler<GetLabels, GetLabelsResult> _getLabels;
        private readonly IQueryHandler<GetProjects, GetProjectsResult> _getProjects;
        private readonly IProfileFactory _profileFactory;

        public ToDoController(IProfileFactory profileFactory,
            IQueryHandler<GetLabels, GetLabelsResult> getLabels,
            IQueryHandler<GetProjects, GetProjectsResult> getProjects)
        {
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
            //TODO:  I am pretty sure I can get these in a single query and just map them differently since they come from same table with index overloading. 
            //TODO:  We need some better integration testing here to verify that we are querying the right thing.
            _getLabels = getLabels ?? throw new ArgumentNullException(nameof(getLabels));
            _getProjects = getProjects ?? throw new ArgumentNullException(nameof(getProjects));
        }

        [Route("/todo")]
        [HttpGet]
        public async Task<IActionResult> ToDo()
        {
            var profile = _profileFactory.GetCurrentProfile();
            var toReturn = new ToDoResult();
            var labelResultOption = await _getLabels.Run(new GetLabels(profile.UserId));
            labelResultOption.Match(some => toReturn.Labels = some.Labels,
                () => toReturn.Labels = new List<ILabel>());

            var projectResultOption = await _getProjects.Run(new GetProjects(profile.UserId));
            projectResultOption.Match(some => toReturn.Projects = some.Projects,
                () => toReturn.Projects = new List<IProject>());

            return Json(toReturn);
        }

        public class ToDoResult
        {
            public IEnumerable<ILabel> Labels;
            public IEnumerable<IProject> Projects;
        }
    }
}