using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Controllers
{
    [Route("/api/user")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IQueryHandler<GetUserQueryArgs, GetUserQueryResult> _getUser;
        private readonly IServiceOptOutcomes<CreateUserServiceArg, CreateUserServiceResult, CreateUserServiceOutcome> _createUser;
        private readonly IServiceOptOutcomes<UpdateUserServiceArgs, UpdateUserServiceResult, UpdateUserService.UpdateUserOutcome> _updateUser;

        public UserController(IQueryHandler<GetUserQueryArgs, GetUserQueryResult> getUser,
            IServiceOptOutcomes<CreateUserServiceArg, CreateUserServiceResult, CreateUserServiceOutcome> createUser,
            IServiceOptOutcomes<UpdateUserServiceArgs, UpdateUserServiceResult, UpdateUserService.UpdateUserOutcome> updateUser)
        {
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
            _createUser = createUser ?? throw new ArgumentNullException(nameof(createUser));
            _updateUser = updateUser ?? throw new ArgumentNullException(nameof(updateUser));
        }

        [HttpGet("/{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            var resultOption = await _getUser.Run(new GetUserQueryArgs(userId));
            return resultOption.Match<IActionResult>(some => new JsonResult(some),
                () => new NotFoundResult());
        }

        public class NewUserValues
        {
            public string ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Post(NewUserValues values)
        {
            var args = new CreateUserServiceArg(values.ID, values.FirstName, values.LastName, values.Email);
            var result = await _createUser.Run(args);

            return result.Match<IActionResult>(
                some => Created(new Uri(null, $"/api/user/{values.ID}"), some.User),
                outcome => Conflict($"User already exists for this id {values.ID}"));
        }

        //Question: Does the ID have to be in the URL for a Put?  Ask around / research
        [HttpPut]
        public async Task<IActionResult> Put(UpdateUserServiceArgs userValues)
        {
            if (userValues == null) return BadRequest("Cannot parse required values.");
            if (string.IsNullOrEmpty(userValues.ID)) return BadRequest("ID is required.");

            var optionalResult = await _updateUser.Run(userValues);

            return optionalResult.Match<IActionResult>(
                some => Ok(some.User), 
                outcome =>
                {
                    if (outcome == UpdateUserService.UpdateUserOutcome.UnprocessableEntity)
                        return UnprocessableEntity();

                    if (outcome == UpdateUserService.UpdateUserOutcome.VersionConflict)
                        return Conflict();

                    return NotFound();

                });
        }
    }
}