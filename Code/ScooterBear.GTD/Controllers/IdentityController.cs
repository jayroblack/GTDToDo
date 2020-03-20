using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ScooterBear.GTD.Controllers
{
    [Authorize]
    public class IdentityController : Controller
    {
        [HttpGet]
        [Route("identity")]
        public IActionResult Index()
        {
            return new JsonResult(from c in User.Claims select new {c.Type, c.Value});
        }
    }
}