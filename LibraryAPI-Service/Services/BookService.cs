using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using AutoMapper;
using Azure.Core;
using LibraryApi_Repository.Entities;
using LibraryApi_Repository.Enums;
using LibraryApi_Repository.Interfaces;
using LibraryApi_Repository.Repositories;
using LibraryAPI_Service.Interfaces;
using LibraryAPI_Service.Models.RequestDto;
using LibraryAPI_Service.Models.ResponseDto;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI_Service.Services
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionHandler _transactionHandler;
        private readonly IBookRepository _bookRepo;
        public BookService(IBookRepository bookRepo, IMapper mapper, ITransactionHandler transactionHandler)
        {
            _bookRepo = bookRepo;
            _mapper = mapper;
            _transactionHandler = transactionHandler;
        }

        public async Task<GenericResponse<dynamic>> Add(AddBookDto addBooks)
        {
            var books = addBooks.Books.Select(x => new Books
            {
                Title = x.Title,
                Author = x.Author,
                DateCreated = DateTime.Now,
                Status = BookStatus.Available,
                IsDeleted = false
            });
            await _bookRepo.AddRange(books);
            return new GenericResponse<dynamic>(true, "SUCCESS", null);
        }

        public Task<GenericResponse<BookListDto>> GetBooks(string? searchString, Pager pager)
        {
            var books = _bookRepo.GetPagedList(x=>(x.Title.Contains(searchString) || string.IsNullOrEmpty(searchString)) && !x.IsDeleted, pager.PageNumber, pager.PageSize);
            bool status = books.Items.Any();
            return Task.FromResult(new GenericResponse<BookListDto>(true, status?"SUCCESS":"No book(s) found",new BookListDto(_mapper.Map<List<BooksDto>>(books.Items), books.TotalCount,books.PageNumber,books.PageSize)));
        }

        public async Task<GenericResponse<dynamic>> ReturnBook(int bookId)
        {
            var books = _bookRepo.Find(x => x.Id==bookId && x.Status!=BookStatus.Borrowed);

            if (books.Any())
            {
                var message = $"Book is currently not borrowed";
                return new GenericResponse<dynamic>(false, message, null);
            }
            var date = DateTime.Now;
            await books.ForEachAsync(x =>
            {
                x.Status = BookStatus.Available;
                x.DateModified = date;
            });
            var transaction = await _transactionHandler.BeginTransaction();
            try
            {
                await _bookRepo.UpdateRange(books);
                await _transactionHandler.CommitTransaction(transaction);
                return new GenericResponse<object>(true, $"Successfully Marked as returned", null);
            }
            catch (DbUpdateConcurrencyException)
            {
                await _transactionHandler.RollbackTransaction(transaction);
                return new GenericResponse<object>(false, "Error completing request, please try again", null);
            }
        }
    }
}
