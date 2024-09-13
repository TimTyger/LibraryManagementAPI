using LibraryApi_Repository.Entities;

namespace LibraryAPI_Service.Interfaces
{
    public interface ITokenService
    {
        Task GenerateToken(User user);
    }
}