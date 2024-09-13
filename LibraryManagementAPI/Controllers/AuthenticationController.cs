using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using LibraryAPI_Service.Interfaces;
using LibraryAPI_Service.Models.ResponseDto;

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

        /// <summary>
        /// Login using google authentication
        /// </summary>
        /// <returns>Redirects to the google authentication page for authentication</returns>
        [HttpGet("login")]
        public IActionResult Login()
        {
            var redirectUrl = Url.Action("LoginWithCallback", "Authentication",null, Request.Scheme, Request.Host.Value);
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// The call back endpoint after google authentication
        /// </summary>
        /// <returns></returns>
        [HttpGet("loginwithcallback")]
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

        /// <summary>
        /// To sign out user
        /// </summary>
        /// <returns></returns>
        [HttpGet("signout")]
        public async Task<IActionResult> LogOut(){
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new GenericResponse<dynamic>(true, "Signed out", null));
        } 
    }
}
