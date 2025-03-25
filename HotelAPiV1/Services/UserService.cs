using HotelBookingApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetByIdAsync(string userId) //  FIXED
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}
