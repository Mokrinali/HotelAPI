using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBookingApp.Models;
using HotelBookingApp.Repositories;

namespace HotelBookingApp.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<List<Hotel>> GetAllHotelsAsync()
        {
            return await _hotelRepository.GetAllHotelsAsync();
        }

        public async Task<Hotel> GetHotelByIdAsync(int id)
        {
            return await _hotelRepository.GetHotelByIdAsync(id);
        }

        public async Task<bool> AddHotelAsync(Hotel hotel)
        {
            return await _hotelRepository.AddHotelAsync(hotel);
        }

        public async Task<bool> UpdateHotelAsync(Hotel hotel)
        {
            return await _hotelRepository.UpdateHotelAsync(hotel);
        }

        public async Task<(bool Success, string Message)> DeleteHotelAsync(int id)
        {
            var hotel = await _hotelRepository.GetHotelByIdAsync(id);

            if (hotel == null)
                return (false, "Hotel not found.");

            if (hotel.Rooms != null && hotel.Rooms.Any())
                return (false, "Cannot delete hotel with existing rooms.");

            var deleted = await _hotelRepository.DeleteHotelAsync(id);

            return deleted
                ? (true, "Hotel deleted successfully.")
                : (false, "Failed to delete hotel.");
        }



    }
}
