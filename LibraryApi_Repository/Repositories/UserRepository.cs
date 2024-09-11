using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApi_Repository.Data;
using LibraryApi_Repository.Entities;
using LibraryApi_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi_Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryApiContext _context;

        public UserRepository(LibraryApiContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string emailClaim)
        {
            return await _context.Users.FirstOrDefaultAsync(x=>x.Email == emailClaim);
        }
        public async Task<User?> GetActiveUserByEmailAsync(string emailClaim)
        {
            return await _context.Users.FirstOrDefaultAsync(x=>x.Email == emailClaim && x.IsDeleted==false);
        }

        public  async Task<User?> AddUser(User user)
        {
            var entityEntry = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

    }
}
