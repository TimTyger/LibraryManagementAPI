using LibraryApi_Repository.Entities;
using LibraryApi_Repository.Enums;
using LibraryApi_Repository.Interfaces;
using LibraryApi_Repository.Repositories;
using LibraryAPI_Service.Interfaces;
using LibraryAPI_Service.Models.RequestDto;
using LibraryAPI_Service.Models.ResponseDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI_Service.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ITransactionHandler _transactionHandler;
        private readonly IReservationRepository _reservationRepo;
        private readonly IUserRepository _userRepository;
        public ReservationService(IReservationRepository reservationRepository, ITransactionHandler transactionHandler, IBookRepository bookRepository, IUserRepository userRepository)
        {
            _reservationRepo = reservationRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _transactionHandler = transactionHandler;
        }
        public async Task<GenericResponse<dynamic>> ReserveBook(ReservationReq request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId)) { return new GenericResponse<object>(false, "Invalid user", null); }
            var user = await _userRepository.GetActiveUserByEmailAsync(request.UserId!);
            if (user == null) { return new GenericResponse<object>(false, "Currently unable to identify user", null); }
            var book =  _bookRepository.GetById(request.BookId);
            if (book == null) { return new GenericResponse<object>(false, "Invalid book id supplied", null); }
            if (book!.Status == BookStatus.Reserved || book.Status==BookStatus.Borrowed) 
            {
                var message = $"Book is currently unavailable";
                if(book.NextAvaillableTime!=null)message += $" until {book.NextAvaillableTime}";
                return new GenericResponse<object>(false, message, null);
            }
            var date= DateTime.Now;
            var reservationReq = new Reservations
            {
                BookId = request.BookId,
                DateCreated = date,
                IsDeleted = false,
                ReservationExpiresAt = date.AddHours(24),
                UserId = user.Id,
            };
            book.Status = BookStatus.Reserved;
            book.DateModified = date;
            book.NextAvaillableTime = date.AddHours(24);
            var transaction = await _transactionHandler.BeginTransaction();
            try
            {
                await _bookRepository.Update(book);
                await _reservationRepo.Add(reservationReq);
                await _transactionHandler.CommitTransaction(transaction);
                return new GenericResponse<object>(true, $"Reservation for '{book.Title}' successful", null);
            }
            catch (DbUpdateConcurrencyException)
            {
                await _transactionHandler.RollbackTransaction(transaction);
                return new GenericResponse<object>(false, "Error completing request, please try again", null);
            }
        }
    }
}
