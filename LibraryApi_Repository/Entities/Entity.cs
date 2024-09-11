using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi_Repository.Entities
{
    public class Entity<T>
    {
        [Key]
        public T Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
    }
}
