using LibraryApi_Repository.Data;
using LibraryApi_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi_Repository.Repositories
{
    public class BookRepository : GenericRepository<Books>, IBookRepository
    {
        public BookRepository(LibraryApiContext context):base(context)
        {
            
        }
    }
}
