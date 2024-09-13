using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI_Service.Interfaces
{
    public interface IMapFrom<T>
    {
        public void Mapping(Profile profile, IHttpContextAccessor httpContextAccessor) => profile.CreateMap(typeof(T), GetType());
    }
}
