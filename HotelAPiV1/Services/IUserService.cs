using HotelBookingApp.Models;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetByIdAsync(string userId); //  FIXED
    }
}
