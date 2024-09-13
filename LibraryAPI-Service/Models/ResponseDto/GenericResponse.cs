using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI_Service.Models.ResponseDto
{
    public class GenericResponse<T>(bool IsSuccess, string Message, T? body) where T : class
    {
        public bool IsSuccess { get; set; } = IsSuccess;
        public string Message { get; set; } = Message;
        public T? Body { get; set; } = body;
    }
}
