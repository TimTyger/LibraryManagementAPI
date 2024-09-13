using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LibraryAPI_Service.Helpers.ValidationHelper;

namespace LibraryAPI_Service.Models.RequestDto
{
    public class Pager
    {
        [GreaterThanZero]public int PageNumber { get; set; } = 1;
        [GreaterThanZero] public int PageSize { get; set; } = 10;
    }
}
