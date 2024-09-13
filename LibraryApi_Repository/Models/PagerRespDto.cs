using LibraryApi_Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi_Repository.Models
{
    public class PagerRespDto<T> : IPagerResponseDto<IQueryable<T>> where T :class
    {
        public int TotalCount { get ; set ; }
        public int PageNumber { get ; set ; }
        public int PageSize { get ; set ; }
        public IQueryable<T> Items { get; set; }
    }
}
