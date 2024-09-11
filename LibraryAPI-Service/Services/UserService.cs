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
using LibraryAPI_Service.Models;
using Microsoft.AspNetCore.Authentication;

namespace LibraryAPI_Service.Services
{
    public class UserService : IUserService, IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
                    Email = email,
                    Name = name,
                    Role = Roles.Customer.ToString()
                });
                await SignUp(emailClaim, nameClaim);
            }
            if (user!.IsDeleted) { }

        }
        
        private async Task SignUp(User user)
        {
             await _userRepository.AddUser(user) ;
        }
    }
}
