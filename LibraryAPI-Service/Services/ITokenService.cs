using LibraryApi_Repository.Entities;

namespace LibraryAPI_Service.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}