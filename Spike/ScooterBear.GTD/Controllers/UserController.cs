using Microsoft.AspNetCore.Mvc;

namespace ScooterBear.GTD.Controllers
{
    [Route("/api/user")]
    public class UserController : Controller
    {
        [HttpGet("{userId}")]
        public IActionResult Get(string userId)
        {
            return new JsonResult($"User for {userId}");
        }
    }
}