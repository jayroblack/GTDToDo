using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ScooterBear.GTD.Controllers
{
    [Authorize]
    public class HealthCheckController : Controller
    {
        [Route("/healthcheck")]
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }
    }
}