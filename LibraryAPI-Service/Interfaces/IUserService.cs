using LibraryAPI_Service.Models;
using Microsoft.AspNetCore.Authentication;

namespace LibraryAPI_Service.Interfaces
{
    public interface IUserService
    {
    }
    
    public interface IAuthService
    {
        Task<GenericResponse<string>> Login(AuthenticateResult authenticateResult);
    }
}