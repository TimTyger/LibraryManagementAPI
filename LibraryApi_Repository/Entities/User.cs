using System.ComponentModel.DataAnnotations;

namespace LibraryApi_Repository.Entities
{
    public class User: Entity<int>
    {
        [Required]
        public string Email { get; set; }
        public string? Name { get; set; }
        [Required]public string Role { get; set; }
    }
}
