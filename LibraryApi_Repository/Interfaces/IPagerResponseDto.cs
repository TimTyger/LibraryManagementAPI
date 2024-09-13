using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi_Repository.Interfaces
{
    public interface IPagerResponseDto<T> where T : class
    {
        int TotalCount { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        T Items { get; set; }
    }
}
