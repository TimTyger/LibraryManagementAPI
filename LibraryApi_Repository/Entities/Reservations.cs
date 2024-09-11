using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApi_Repository.Entities
{
    public class Reservations : Entity<int>
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime ReservationExpiresAt { get; set; }
        [ForeignKey(nameof(BookId))]
        public virtual Books Book { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
