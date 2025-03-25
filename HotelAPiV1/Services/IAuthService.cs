using System.Threading.Tasks;
using HotelBookingApp.DTOs;

namespace HotelBookingApp.Services
{
    public interface IAuthService
    {
        Task<AuthDto> AuthenticateUser(LoginDto loginDto);
        Task<bool> RegisterUser(RegisterDto registerDto);
    }
}
