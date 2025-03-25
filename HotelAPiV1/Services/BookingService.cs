using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBookingApp.Data;
using HotelBookingApp.Models;
using HotelBookingApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApp.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllBookingsAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetBookingByIdAsync(id);
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            return !await _context.Bookings
                .AnyAsync(b => b.RoomId == roomId &&
                               b.CheckInDate < checkOut &&
                               b.CheckOutDate > checkIn);
        }
        public async Task<bool> CreateBookingAsync(Booking booking)
        {
            var isAvailable = await IsRoomAvailableAsync(booking.RoomId, booking.CheckInDate, booking.CheckOutDate);
            if (!isAvailable)
                return false;
            booking.IsConfirmed = true;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            return await _bookingRepository.DeleteBookingAsync(id);
        }
        public IEnumerable<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut)
        {
            return _context.Rooms
                .Where(r => !_context.Bookings.Any(b =>
                    b.RoomId == r.Id &&
                    b.CheckInDate < checkOut &&
                    b.CheckOutDate > checkIn))
                .ToList();
        }

    }
}
