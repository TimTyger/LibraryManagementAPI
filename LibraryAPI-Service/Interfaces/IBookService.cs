using LibraryAPI_Service.Models.RequestDto;
using LibraryAPI_Service.Models.ResponseDto;

namespace LibraryAPI_Service.Interfaces
{
    public interface IBookService
    {
        Task<GenericResponse<List<BooksDto>>> GetBooks(string? searchString);
        Task<GenericResponse<dynamic>> Add(AddBookDto books);
        Task<GenericResponse<dynamic>> ReturnBook(int bookId);
    }
}