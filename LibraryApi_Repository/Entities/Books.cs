using LibraryApi_Repository.Enums;
using System.ComponentModel.DataAnnotations;

namespace LibraryApi_Repository.Entities
{
    public class Books : Entity<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public BookStatus Status {  get; set; } = BookStatus.Available;
        public DateTime? NextAvaillableTime { get; set; } = null;

        [Timestamp]
        public byte[] Version { get; set; }
    }
}
