using Microsoft.AspNetCore.Mvc;

namespace ScooterBear.GTD.Controllers
{
    [Route("/api")]
    public class TaskController : Controller
    {
        [HttpGet("/{userId}/tasks")]
        public IActionResult Get(string userId)
        {
            return new JsonResult($"Tasks for user {userId}");
        }

        [HttpPost("/{userId}/tasks")]
        public IActionResult Post(string userId)
        {
            return new JsonResult($"Add Task for user {userId}");
        }
    }
}
