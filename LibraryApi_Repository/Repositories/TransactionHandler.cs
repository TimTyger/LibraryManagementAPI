using LibraryApi_Repository.Data;
using LibraryApi_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi_Repository.Repositories
{
    public class TransactionHandler : ITransactionHandler
    {
        protected readonly LibraryApiContext _context;

        public TransactionHandler(LibraryApiContext context)
        {
            _context = context;

        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitTransaction(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
        }
        public async Task RollbackTransaction(IDbContextTransaction transaction)
        {
            await transaction.RollbackAsync();
        }
    }
}
