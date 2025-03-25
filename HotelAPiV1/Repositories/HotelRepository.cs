using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBookingApp.Data;
using HotelBookingApp.Models;

namespace HotelBookingApp.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly ApplicationDbContext _context;

        public HotelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Hotel>> GetAllHotelsAsync()
        {
            return await _context.Hotels.Include(h => h.Rooms).ToListAsync();
        }

        public async Task<Hotel> GetHotelByIdAsync(int id)
        {
            return await _context.Hotels.Include(h => h.Rooms).FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<bool> AddHotelAsync(Hotel hotel)
        {
            await _context.Hotels.AddAsync(hotel);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateHotelAsync(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteHotelAsync(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null) return false;

            _context.Hotels.Remove(hotel);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
