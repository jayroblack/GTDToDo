﻿using Microsoft.AspNetCore.Mvc;

namespace ScooterBear.GTD.Controllers
{
    [Route("/api")]
    public class LabelController : Controller
    {
        [HttpGet("/{userId}/labels")]
        public IActionResult Get(string userId)
        {
            return new JsonResult($"Labels for user {userId}");
        }
    }
}
