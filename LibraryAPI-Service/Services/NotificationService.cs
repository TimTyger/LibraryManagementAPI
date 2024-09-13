using LibraryApi_Repository.Entities;
using LibraryApi_Repository.Enums;
using LibraryApi_Repository.Interfaces;
using LibraryApi_Repository.Repositories;
using LibraryAPI_Service.Interfaces;
using LibraryAPI_Service.Models.RequestDto;
using LibraryAPI_Service.Models.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI_Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepo _notificationRepo;
        private readonly IBookRepository _bookRepo;
        public NotificationService(INotificationRepo notificationRepo, IBookRepository bookRepo)
        {
            _notificationRepo = notificationRepo;
            _bookRepo = bookRepo;
        }

        public  async Task<GenericResponse<dynamic>> AddNotification(ReservationReq request)
        {
            var isBookAvailable = _bookRepo.Find(x=> x.Id==request.BookId && x.Status == BookStatus.Available);
            if (isBookAvailable.Any())
            {
                return new GenericResponse<dynamic>(false, "Book is currently available", null);
            }
            await _notificationRepo.Add(new Notifications
            {
                DateCreated = DateTime.Now,
                IsDeleted = false,
                BookId = request.BookId,
                UserId = request.UserId,
                IsNotified = false
            });
            return new GenericResponse<dynamic>(true,"Notification Created Successfully", null);
        }
    }
}
