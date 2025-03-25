using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.DTOs;
using HotelBookingApp.Services;
using HotelBookingApp.Models;
using HotelBookingApp.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace HotelBookingApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookingService _bookingService;

        public BookingsController(ApplicationDbContext context, IBookingService bookingService)
        {
            _context = context;
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingDto bookingDto)
        {
            var room = await _context.Rooms.FindAsync(bookingDto.RoomId);
            if (room == null)
                return NotFound(new { message = "Room not found." });

            bookingDto.PricePerNight = room.PricePerNight;
            bookingDto.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (bookingDto.CheckOutDate <= bookingDto.CheckInDate)
                return BadRequest(new { message = "Check-out date must be after check-in date." });

            var nights = (bookingDto.CheckOutDate - bookingDto.CheckInDate).Days;
            bookingDto.TotalPrice = nights * bookingDto.PricePerNight;

            var booking = new Booking
            {
                UserId = bookingDto.UserId,
                HotelId = bookingDto.HotelId,
                RoomId = bookingDto.RoomId,
                CheckInDate = bookingDto.CheckInDate,
                CheckOutDate = bookingDto.CheckOutDate,
                TotalPrice = bookingDto.TotalPrice,
                IsConfirmed = true
            };

            var result = await _bookingService.CreateBookingAsync(booking);
            if (!result)
                return BadRequest(new { message = "Failed to create booking." });

            return Ok(new { message = "Booking created successfully." });
        }

        [HttpGet("available")]
        public IActionResult AvailableRooms([FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut)
        {
            var availableRooms = _bookingService.GetAvailableRooms(checkIn, checkOut);
            return Ok(availableRooms);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserBookings()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Hotel)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Hotel)
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (booking == null)
                return NotFound();

            var dto = new BookingDto
            {
                Id = booking.Id,
                RoomId = booking.RoomId,
                RoomName = booking.Room?.Name ?? "N/A",
                HotelId = booking.HotelId,
                HotelName = booking.Hotel?.Name ?? "N/A",
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                TotalPrice = booking.TotalPrice,
                PricePerNight = booking.Room?.PricePerNight ?? 0
            };

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] BookingDto bookingDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
            if (booking == null)
                return NotFound();

            var nights = (bookingDto.CheckOutDate - bookingDto.CheckInDate).Days;
            var room = await _context.Rooms.FindAsync(bookingDto.RoomId);

            booking.CheckInDate = bookingDto.CheckInDate;
            booking.CheckOutDate = bookingDto.CheckOutDate;
            booking.TotalPrice = nights * (room?.PricePerNight ?? 0);

            await _context.SaveChangesAsync();
            return Ok(new { message = "Booking updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
            if (booking == null)
                return NotFound();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking deleted successfully." });
        }
    }
}
