using LibraryApi_Repository.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibraryAPI_Service.Models.RequestDto
{
    public class AddBookDto
    {
        [Required,MinLength(1, ErrorMessage = "The Books list must contain at least one book.")]
        public List<AddBookReq> Books { get; set; } = [];
    }
    public class AddBookReq
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; } = string.Empty;
        [JsonIgnore]
        public BookStatus Status { get; set; }
    }
}
