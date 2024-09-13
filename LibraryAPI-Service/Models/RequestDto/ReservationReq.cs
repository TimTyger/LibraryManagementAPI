using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static LibraryAPI_Service.Helpers.ValidationHelper;

namespace LibraryAPI_Service.Models.RequestDto
{
    public class ReservationReq
    {
        [Required(ErrorMessage ="Book id is required"), GreaterThanZero]
        public int BookId { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
