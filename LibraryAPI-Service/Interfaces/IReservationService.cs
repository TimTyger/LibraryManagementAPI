using LibraryAPI_Service.Models.RequestDto;
using LibraryAPI_Service.Models.ResponseDto;

namespace LibraryAPI_Service.Interfaces
{
    public interface IReservationService
    {
        Task<GenericResponse<dynamic>> ReserveBook(ReservationReq request);
    }
}