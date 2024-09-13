using LibraryApi_Repository.Data;
using LibraryApi_Repository.Entities;
using LibraryApi_Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi_Repository.Repositories
{
    public class ReservationRepository : GenericRepository<Reservations>, IReservationRepository
    {
        public ReservationRepository(LibraryApiContext context) : base(context)
        {
        }

    }
}
