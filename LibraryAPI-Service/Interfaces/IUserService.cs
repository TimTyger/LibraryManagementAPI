using LibraryAPI_Service.Models.ResponseDto;
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