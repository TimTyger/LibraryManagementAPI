using System.ComponentModel.DataAnnotations;

namespace LibraryApi_Repository.Entities
{
    public class Books : Entity<int>
    {
        public string Title { get; set; } = string.Empty;
        public bool IsReserved { get; set; } = false;
        public bool IsBorrowed { get; set; } = false;
        public DateTime? ReservedUntil { get; set; } = null;

        [Timestamp]
        public byte[] Version { get; set; }
    }
}
