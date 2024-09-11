using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using LibraryAPI_Service.Interfaces;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authenticationService)
        {
            _authService = authenticationService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var redirectUrl = Url.Action("LoginWithCallback", "Authentication");
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("LoginWithCallback")]
        public async Task<IActionResult> LoginWithCallback()
        {
            // Authenticate the user
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return BadRequest("Authentication failed.");
            }
            var response = await _authService.Login(authenticateResult);            

            return Ok( response);
        }

    }
}
