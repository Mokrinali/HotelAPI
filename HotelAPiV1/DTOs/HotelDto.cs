using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingApp.DTOs
{
    public class HotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public IFormFile? ImageFile { get; set; }


        [Required(ErrorMessage = "Please upload an image")]
        public string FeaturedImage { get; set; }
        public List<RoomDto> Rooms { get; set; } = new List<RoomDto>();
    }

}
