using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Description { get; set; }

        public double Rating { get; set; } // e.g., 4.5 stars

        public string FeaturedImage { get; set; } // Path to image

        public List<Room> Rooms { get; set; }
    }
}
