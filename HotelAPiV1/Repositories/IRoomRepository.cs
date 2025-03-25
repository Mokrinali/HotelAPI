using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBookingApp.Models;

namespace HotelBookingApp.Repositories
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAllRoomsAsync();
        Task<Room> GetRoomByIdAsync(int id);
        Task<bool> AddRoomAsync(Room room);
        Task<bool> UpdateRoomAsync(Room room);
        Task<bool> DeleteRoomAsync(int id);
        Task<List<Room>> GetRoomsByHotelIdAsync(int hotelId);

    }
}
