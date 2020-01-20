﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScooterBear.GTD.Application.Users;
using ScooterBear.GTD.Application.Users.New;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.Controllers
{
    [Route("/api/user")]
    public class UserController : Controller
    {
        private readonly IQueryHandlerAsync<GetUserQueryArgs, GetUserQueryResult> _getUser;
        private readonly IServiceAsync<CreateUserServiceArg, CreateUserServiceResult> _createUser;
        
        public UserController(IQueryHandlerAsync<GetUserQueryArgs, GetUserQueryResult> getUser,
            IServiceAsync<CreateUserServiceArg, CreateUserServiceResult> createUser)
        {
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
            _createUser = createUser ?? throw new ArgumentNullException(nameof(createUser));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            var resultOption = await _getUser.Run(new GetUserQueryArgs(userId));
            return resultOption.Match<IActionResult>( some => new JsonResult(some) ,
                 () => new NotFoundResult());
        }

        public class NewUserValues
        {
            public string ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }

        //TODO:  Check for Existing Item - Return 409 Conflict if exists. - After you return a user;
        //TODO:  Create Global Error Handler that maps to return codes - ArgumentException should return 400 or 422
        [HttpPost]
        public async Task<IActionResult> Post(NewUserValues values)
        {
            var args = new CreateUserServiceArg(values.ID, values.FirstName, values.LastName, values.Email);
            var result = await _createUser.Run(args);
            return Created(new Uri(null, $"/api/user/{values.ID}"), result.User);
        }

        [HttpPut]
        public async Task<IActionResult> Put()
        {
            //Replaces the item at the key.  
            // If it does not exist return 404.
            // If it exists, but the version numbers don't match return 409 conflict. 
            // If Succeeds return 202 Accepted.
            throw new NotImplementedException();
        }
    }
}