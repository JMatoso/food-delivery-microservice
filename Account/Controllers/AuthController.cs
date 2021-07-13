using System.Threading.Tasks;
using Account.Data;
using Account.Data.Repositories;
using Account.Models;
using Account.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Account.Controllers
{
    /// <summary>
    /// Authentication Service.
    /// </summary>
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepo _user;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthController(
            IUserRepo user,
            SignInManager<ApplicationUser> signInManager)
        {
            _user = user;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Login.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="400">Fill all required fields.</response>
        /// <response code="401">Unauthorized.</response>
        [HttpPost("auth")]
        public async Task<ActionResult<TokenReturned>> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    var user = await _user.Get(model.Email);
                    return TokenService.GenerateToken(user);
                }
                else if(result.IsLockedOut)
                {
                    return Unauthorized("User is currently locked out.");
                }
                else
                {
                    return Unauthorized("Wrong Credentials.");
                }
            }
            else
            {
                return BadRequest("Fill all required fields.");
            }
        }
    }
}