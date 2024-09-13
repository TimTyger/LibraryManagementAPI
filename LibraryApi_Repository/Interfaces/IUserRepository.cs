
using LibraryApi_Repository.Entities;

namespace LibraryApi_Repository.Interfaces
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string emailClaim);
        Task<User?> GetActiveUserByEmailAsync(string emailClaim);
    }
}