using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBookingApp.Models;

namespace HotelBookingApp.Services
{
    public interface IBookingService
    {
        Task<List<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        Task<bool> CreateBookingAsync(Booking booking);
        Task<bool> DeleteBookingAsync(int id);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut);
        IEnumerable<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut);

    }
}
