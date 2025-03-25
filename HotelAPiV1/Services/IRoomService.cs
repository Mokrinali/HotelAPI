using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBookingApp.DTOs;
using HotelBookingApp.Models;
//using HotelBookingApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApp.Services
{
    public interface IRoomService
    {
        Task<List<RoomDto>> GetAllRoomsAsync();
        Task<List<RoomDto>> GetRoomsByHotelIdAsync(int hotelId);
        Task<RoomDto> GetRoomByIdAsync(int id);
        Task<bool> AddRoomAsync(Room room);
        Task<bool> UpdateRoomAsync(Room room);
        Task<bool> DeleteRoomAsync(int id);
        Task<Room> GetRoomEntityByIdAsync(int id);

    }
}
