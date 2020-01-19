using Microsoft.AspNetCore.Mvc;

namespace ScooterBear.GTD.Controllers
{
    [Route("/api")]
    public class ProjectController : Controller
    {
        [HttpGet("/{userId}/projects")]
        public IActionResult Get(string userId)
        {
            return new JsonResult($"Projects for user {userId}");
        }

        [HttpPost("/{userId}/projects")]
        public IActionResult Post(string userId)
        {
            return new JsonResult($"Add Project for user {userId}");
        }
    }
}
