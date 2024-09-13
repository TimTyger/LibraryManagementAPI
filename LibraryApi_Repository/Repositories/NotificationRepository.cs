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
    public class NotificationRepository : GenericRepository<Notifications>, INotificationRepo
    {
        public NotificationRepository(LibraryApiContext context) : base(context)
        {
        }
    }
}
