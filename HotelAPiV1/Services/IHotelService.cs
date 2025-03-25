using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBookingApp.Models;

namespace HotelBookingApp.Services
{
    public interface IHotelService
    {
        Task<List<Hotel>> GetAllHotelsAsync();
        Task<Hotel> GetHotelByIdAsync(int id);
        Task<bool> AddHotelAsync(Hotel hotel);
        Task<bool> UpdateHotelAsync(Hotel hotel);
        Task<(bool Success, string Message)> DeleteHotelAsync(int id);

    }
}
