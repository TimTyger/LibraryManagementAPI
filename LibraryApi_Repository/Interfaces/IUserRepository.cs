
using LibraryApi_Repository.Entities;

namespace LibraryApi_Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string emailClaim);
        Task<User?> GetActiveUserByEmailAsync(string emailClaim);
        Task<User?> AddUser(User user);
        Task UpdateUser(User user);
    }
}