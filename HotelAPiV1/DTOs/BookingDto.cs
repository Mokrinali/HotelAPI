using System;

namespace HotelBookingApp.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal PricePerNight { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
