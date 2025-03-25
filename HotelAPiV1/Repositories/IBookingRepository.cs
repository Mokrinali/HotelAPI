using HotelBookingApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp.Repositories
{
    public interface IBookingRepository
    {
        Task<bool> CreateBookingAsync(Booking booking);
        Task<Booking> GetBookingByIdAsync(int id);
        Task<List<Booking>> GetAllBookingsAsync();
        Task<List<Booking>> GetBookingsByUserIdAsync(string userId);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate);
        Task<bool> DeleteBookingAsync(int id); // Add this method
    }
}
