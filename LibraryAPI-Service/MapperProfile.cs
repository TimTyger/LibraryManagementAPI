using AutoMapper;
using LibraryApi_Repository.Entities;
using LibraryAPI_Service.Interfaces;
using LibraryAPI_Service.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI_Service
{
    public class MapperProfile:Profile
    {
        private readonly IHttpContextAccessor _httpAccessor;
        public MapperProfile(IHttpContextAccessor httpContextAccessor, Assembly[] assembly)
        {
            _httpAccessor = httpContextAccessor;
            ApplyMappings(assembly);
        }
        private void ApplyMappings(Assembly[] assembly) 
        {
            var types = assembly.SelectMany(a=>a.GetExportedTypes().Where(x=>x.GetInterfaces().Any(y=>y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IMapFrom<>)))).ToList();
            foreach (var type in types) 
            {
                var instance = Activator.CreateInstance(type);
                var methodinfo = type.GetMethod("Mapping") ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");
                methodinfo?.Invoke(instance, [this, _httpAccessor]);
            }
        }
    }
}
