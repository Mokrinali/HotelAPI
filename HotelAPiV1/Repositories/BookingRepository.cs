using HotelBookingApp.Data;
using HotelBookingApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BookingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ===========================
        //       ADD NEW BOOKING
        // ===========================
        public async Task<bool> CreateBookingAsync(Booking booking)
        {
            _dbContext.Bookings.Add(booking);
            return await _dbContext.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteBookingAsync(int id)
        {
            var booking = await _dbContext.Bookings.FindAsync(id);
            if (booking == null)
                return false;

            _dbContext.Bookings.Remove(booking);
            return await _dbContext.SaveChangesAsync() > 0;
        }


        // ===========================
        //     GET BOOKING BY ID
        // ===========================
        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _dbContext.Bookings
                .Include(b => b.Hotel)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        // ===========================
        //     GET ALL BOOKINGS
        // ===========================
        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _dbContext.Bookings
                .Include(b => b.Hotel)
                .Include(b => b.Room)
                .ToListAsync();
        }

        // ===========================
        // GET BOOKINGS BY USER ID
        // ===========================
        public async Task<List<Booking>> GetBookingsByUserIdAsync(string userId)
        {
            return await _dbContext.Bookings
                .Include(b => b.Hotel)
                .Include(b => b.Room)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        // ===========================
        // CHECK IF ROOM IS AVAILABLE
        // ===========================
        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            return !await _dbContext.Bookings.AnyAsync(b =>
                b.RoomId == roomId &&
                ((b.CheckInDate <= checkOutDate && b.CheckOutDate >= checkInDate) ||
                 (b.CheckInDate >= checkInDate && b.CheckInDate <= checkOutDate)));
        }

    }
}
