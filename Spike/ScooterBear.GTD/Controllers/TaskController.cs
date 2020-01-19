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
    }
}
