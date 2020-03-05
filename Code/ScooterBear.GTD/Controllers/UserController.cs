using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Application.Users.Update;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IQueryHandler<GetUserQueryArgs, GetUserQueryResult> _getUser;
        private readonly IServiceOptOutcomes<CreateUserServiceArg, CreateUserServiceResult, CreateUserServiceOutcome> _createUser;
        private readonly IServiceOptOutcomes<UpdateUserServiceArgs, UpdateUserServiceResult, UpdateUserService.UpdateUserOutcome> _updateUser;
        private readonly IServiceOpt<GetOrCreateUserServiceArgs, GetOrCreateUserServiceResult> _getOrCreateUser;

        public UserController(IQueryHandler<GetUserQueryArgs, GetUserQueryResult> getUser,
            IServiceOptOutcomes<CreateUserServiceArg, CreateUserServiceResult, CreateUserServiceOutcome> createUser,
            IServiceOptOutcomes<UpdateUserServiceArgs, UpdateUserServiceResult, UpdateUserService.UpdateUserOutcome> updateUser,
            IServiceOpt<GetOrCreateUserServiceArgs, GetOrCreateUserServiceResult> getOrCreateUser)
        {
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
            _createUser = createUser ?? throw new ArgumentNullException(nameof(createUser));
            _updateUser = updateUser ?? throw new ArgumentNullException(nameof(updateUser));
            _getOrCreateUser = getOrCreateUser ?? throw new ArgumentNullException(nameof(getOrCreateUser));
        }

        [HttpPost]
        [Route("/users/GetOrCreateUser")]
        public async Task<IActionResult> GetOrCreateUser([FromBody] NewUserValues values)
        {
            var resultOption =
                await _getOrCreateUser.Run(new GetOrCreateUserServiceArgs(values.ID, values.FirstName, values.LastName,
                    values.Email));

            var result = resultOption.Match<IActionResult>(some => Json(some.User), UnprocessableEntity);

            return result;
        }

        [HttpGet]
        [Route("/user/{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            var resultOption = await _getUser.Run(new GetUserQueryArgs(userId));
            return resultOption.Match<IActionResult>(some => new JsonResult(some),
                () => new NotFoundResult());
        }

        public class NewUserValues
        {
            [JsonProperty("id")]
            public string ID { get; set; }
            [JsonProperty("firstName")]
            public string FirstName { get; set; }
            [JsonProperty("lastName")]
            public string LastName { get; set; }
            [JsonProperty("email")]
            public string Email { get; set; }
        }

        [HttpPost]
        [Route("/user")]
        public async Task<IActionResult> Post(NewUserValues values)
        {
            var args = new CreateUserServiceArg(values.ID, values.FirstName, values.LastName, values.Email);
            var result = await _createUser.Run(args);

            return result.Match<IActionResult>(
                some => Created(new Uri(null, $"/api/user/{values.ID}"), some.User),
                outcome => Conflict($"User already exists for this id {values.ID}"));
        }

        [HttpPut]
        [Route("user/{userId}")]
        public async Task<IActionResult> Put([FromRoute]string userId, [FromBody]UpdateUserServiceArgs userValues)
        {
            if (userValues == null) return BadRequest("Cannot parse required values.");
            if (string.IsNullOrEmpty(userValues.ID)) return BadRequest("ID is required.");

            var optionalResult = await _updateUser.Run(userValues);

            return optionalResult.Match<IActionResult>(
                some => Ok(some.User), 
                outcome =>
                {
                    if (outcome == UpdateUserService.UpdateUserOutcome.UnprocessableEntity)
                        return UnprocessableEntity(outcome.ToString());

                    if (outcome == UpdateUserService.UpdateUserOutcome.VersionConflict)
                        return Conflict("Version of the user does not match the version stored.  Get a fresh copy and try again.");

                    return NotFound("Could not find the user with this Id.");

                });
        }
    }
}