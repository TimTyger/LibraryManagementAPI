using AutoMapper;
using LibraryApi_Repository.Entities;
using LibraryAPI_Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LibraryAPI_Service.Models.ResponseDto
{
    public class BooksDto : IMapFrom<Books>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime? NextAvaillableTime { get; set; } = null;

        public void Mapping(Profile profile, IHttpContextAccessor httpContextAccessor)
        {
            profile.CreateMap<Books, BooksDto>()
                .ForMember(x => x.Status, i => i.MapFrom(a => a.Status.ToString()));
        }
    }

    public class BookListDto : IPagerResponse
    {
        public BookListDto(List<BooksDto> books, int totaCount, int pageNumber, int pageSize)
        {
            TotalCount = totaCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Books = books;
        }
        public int TotalCount { get ; set ; }
        public int PageNumber { get ; set ; }
        public int PageSize { get ; set ; }
        public List<BooksDto> Books { get; set; } = [];
    }
}