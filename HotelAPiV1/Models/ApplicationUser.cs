using Microsoft.AspNetCore.Identity;

namespace HotelBookingApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
