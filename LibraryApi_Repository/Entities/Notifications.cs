namespace LibraryApi_Repository.Entities
{
    public class Notifications : Entity<int>
    {
        public int BookId { get; set; }
        public string? UserId { get; set; }
        public bool IsNotified { get; set; } = false;
    }
}
