using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBookingApp.Models;

namespace HotelBookingApp.Services
{
    public interface IRoomTypeService
    {
        Task<List<RoomType>> GetAllAsync();
    }
}
