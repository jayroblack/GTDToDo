using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Optional.Async.Extensions;
using ScooterBear.GTD.Abstractions.Users;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Controllers
{
    [Route("/api/user")]
    public class UserController : Controller
    {
        private readonly IQueryHandlerAsync<GetUserQueryArgs, GetUserQueryResult> _getUser;

        public UserController(IQueryHandlerAsync<GetUserQueryArgs, GetUserQueryResult> getUser)
        {
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            var resultOption = await _getUser.Run(new GetUserQueryArgs(userId));
            return resultOption.Match<IActionResult>( some => new JsonResult(some) ,
                 () => new NotFoundResult());
        }

        [HttpPut("{userId}")]
        public IActionResult Put(string userId)
        {
            return new JsonResult($"User for {userId}");
        }
    }
}