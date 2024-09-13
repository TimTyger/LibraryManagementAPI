using LibraryAPI_Service.Models.RequestDto;
using LibraryAPI_Service.Models.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI_Service.Interfaces
{
    public interface INotificationService
    {
        Task<GenericResponse<dynamic>> AddNotification(ReservationReq request);
    }
}
