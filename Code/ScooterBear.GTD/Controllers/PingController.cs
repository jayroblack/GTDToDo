using Microsoft.AspNetCore.Mvc;

namespace ScooterBear.GTD.Controllers
{
    public class PingController : Controller
    {
        [HttpGet]
        [Route("ping")]
        public IActionResult Get()
        {
            return Json("pong");
        }
    }
}