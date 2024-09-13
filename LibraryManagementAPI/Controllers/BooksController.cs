using Azure;
using Azure.Core;
using LibraryApi_Repository.Entities;
using LibraryAPI_Service.Interfaces;
using LibraryAPI_Service.Models.RequestDto;
using LibraryAPI_Service.Models.ResponseDto;
using LibraryAPI_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManagementAPI.Controllers
{
    /// <summary>
    /// Book controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IReservationService _reservationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Book controller constructor
        /// </summary>
        /// <param name="bookService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="reservationService"></param>
        /// <param name="notificationService"></param>
        public BooksController(IBookService bookService, IHttpContextAccessor httpContextAccessor, IReservationService reservationService, INotificationService notificationService)
        {
            _bookService = bookService;
            _reservationService = reservationService;
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
        }

        /// <summary>
        /// To get paginated books, optionally searchstring can be used to search by book title
        /// </summary>
        /// <param name="searchString">The search string to filter books by title.</param>
        /// <param name="pager">Pagination details</param>
        /// <returns>A list of books that match the search criteria.</returns>
        [HttpGet("getbooks"), /*Authorize(Policy = "Customer")*/]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GenericResponse<List<BooksDto>>))]
        public async Task<ActionResult> GetBooks([FromQuery] string? searchString, [FromQuery] Pager pager)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var books = await _bookService.GetBooks(searchString,pager);
            return Ok(books);
        }
        /// <summary>
        /// For reservation of books(by customers)
        /// </summary>
        /// <param name="request">An object containing the Id of the book to be reserved</param>
        /// <returns>A success if reservation is book is still available for reservation</returns>
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GenericResponse<dynamic>))]
        [HttpPost("reserve"),Authorize(Policy = "Customer")]
        public async Task<IActionResult> ReserveBook(ReservationReq request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.UserId = _httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var response = await _reservationService.ReserveBook(request);
            return Ok(response);
        }
        
        /// <summary>
        /// To request for notification when book becomes available(by customers)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GenericResponse<dynamic>))]
        [HttpPost("notify"),Authorize(Policy = "Customer")]
        public async Task<IActionResult> NotifyWhenAvailable(ReservationReq request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.UserId = _httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var response = await _notificationService.AddNotification(request);
            return Ok(response);
        }

        
        /// <summary>
        /// To logged returned books(by librarian)
        /// </summary>
        /// <param name="bookId">the Id of the book returned</param>
        /// <returns></returns>
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GenericResponse<dynamic>))]
        [HttpPut("{bookId}/return"), Authorize(Policy = "AppOwner")]
        public async Task<IActionResult> MarkBookAsReturned([FromRoute]int bookId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _bookService.ReturnBook(bookId);
            return Ok(response);
        }
        
        /// <summary>
        /// To add books
        /// </summary>
        /// <param name="books"></param>
        /// <returns></returns>
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GenericResponse<dynamic>))]
        [HttpPost("addbooks"), Authorize(Policy = "AppOwner")]
        public async Task<IActionResult> AddBooks([FromBody]AddBookDto books)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _bookService.Add(books);
            return Ok(response);
        }




    }
}
