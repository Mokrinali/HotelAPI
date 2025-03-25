using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HotelBookingApp.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int HotelId { get; set; }

        [Required]
        public int RoomTypeId { get; set; }

        [Required]
        [Range(1, 100)]
        public int MaximumGuests { get; set; }

        [Required]
        [Range(1, 10000)]
        public decimal PricePerNight { get; set; }

        public bool IsAvailable { get; set; }

        public IFormFile? ImageFile { get; set; }

        public string? ImagePath { get; set; }

        // ✅ Additional fields for API display
        public string? HotelName { get; set; }

        public string? RoomTypeName { get; set; }
    }
}
