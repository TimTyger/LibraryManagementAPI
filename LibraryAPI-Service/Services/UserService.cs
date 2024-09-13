using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LibraryApi_Repository.Entities;
using LibraryApi_Repository.Interfaces;
using LibraryAPI_Service.Enums;
using LibraryAPI_Service.Interfaces;
using LibraryAPI_Service.Models.ResponseDto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;

namespace LibraryAPI_Service.Services
{
    public class UserService : IUserService, IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        private string[] adminUsers;
        public UserService(IUserRepository userRepository, ITokenService tokenService, IConfiguration config)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _config = config;
            adminUsers = _config["AppOwners"]!.Split(',');
        }

        public async Task<GenericResponse<string>> Login(AuthenticateResult authenticateResult)
        {
            // Extract user information from the authentication result
            var claims = authenticateResult!.Principal!.Identities.FirstOrDefault()?.Claims;
            var emailClaim = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var nameClaim = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(emailClaim))
            {
                return new GenericResponse<string>(false, "Error retrieving email", "Error Occured, please try again later");
            }
            // Check if user exists in DB
            var user = await _userRepository.GetUserByEmailAsync(emailClaim);
            if (user == null)
            {
                user = new User
                {
                    Email = emailClaim,
                    Name = nameClaim,
                    Role = adminUsers.Contains(emailClaim)?Roles.AppOwner.ToString():Roles.Customer.ToString()
                };
                await SignUp(user);
            }
            else if (user!.IsDeleted) { return new GenericResponse<string>(false, "Kindly contact system admin", ""); }
            await _tokenService.GenerateToken(user);
            return new GenericResponse<string>(true, "Login Successful", "");
        }
        
        private async Task SignUp(User user)
        {
             await _userRepository.Add(user) ;
        }
    }
}
