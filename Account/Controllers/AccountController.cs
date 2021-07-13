using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Account.Data;
using Account.Data.Repositories;
using Account.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Account.Controllers
{
    /// <summary>
    /// Account Management.
    /// </summary>
    [ApiController]
    [Route("api")]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepo _user;
        public AccountController(IUserRepo user)
        {
            _user = user;
        }

        /// <summary>
        /// List all users.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="404">Any user found.</response>
        [HttpGet("account")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ApplicationUser>>> All()
        {
            var users = await _user.Get();
            return users == null ? NotFound("Any user found.") : Ok(users);
        } 

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="400">Fill all required fields.</response>
        [HttpPost("account")]
        public async Task<ActionResult> Create(User model)
        {
            if(ModelState.IsValid)
            {
                return await _user.Add(model) == true ? Ok("User created successfully.") : BadRequest("Something went wrong, try again.");
            }

            return BadRequest("Fill all required fields.");
        }

        /// <summary>
        /// Select a user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid email.</response>
        /// <response code="404">Not Found.</response>
        [HttpGet("account/i/{userId}")]
        public async Task<ActionResult<ApplicationUser>> Get(Guid userId)
        {
            if(Guid.Empty == userId)
            {
                return BadRequest("Insert a valid user id.");
            }

            var user = await _user.Get(userId);
            user.PasswordHash = string.Empty;

            return user == null ? NotFound("User not found.") : Ok(user);
        }

        /// <summary>
        /// Select a user.
        /// </summary>
        /// <param name="userEmail">User email.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid email.</response>
        /// <response code="404">Not Found.</response>
        [HttpGet("account/e/{userEmail}")]
        public async Task<ActionResult<ApplicationUser>> Get(string userEmail)
        {
            if(string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("Insert a valid user email.");
            }

            var user = await _user.Get(userEmail);
            user.PasswordHash = string.Empty;

            return user == null ? NotFound("User not found.") : Ok(user);
        }

        /// <summary>
        /// Remove a user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <response code="200">Ok.</response>
        /// <response code="400">Insert a valid user id.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="404">Not Found.</response>
        [HttpDelete("account/{userId}")]
        //[Authorize]
        public async Task<ActionResult<ApplicationUser>> Remove(Guid userId)
        {
            if(Guid.Empty == userId)
            {
                return BadRequest("Insert a valid user id.");
            }

            return await _user.Remove(userId) == false ? NotFound("User not found or something went wrong!") : Ok("User has been removed.");
        }
    }
}