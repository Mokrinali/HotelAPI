using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HotelBookingApp.Models;
using HotelBookingApp.Services;
using HotelBookingApp.DTOs;

namespace HotelBookingApp.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;

        public AdminController(IHotelService hotelService, IRoomService roomService)
        {
            _hotelService = hotelService;
            _roomService = roomService;
        }

        // ===========================
        //        HOTELS MANAGEMENT
        // ===========================

        [HttpGet("hotels")]
        public async Task<IActionResult> GetHotels()
        {
            var hotels = await _hotelService.GetAllHotelsAsync();
            return Ok(hotels);
        }

        [HttpPost("hotels")]
        public async Task<IActionResult> CreateHotel([FromBody] HotelDto hotelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = new Hotel
            {
                Name = hotelDto.Name,
                Address = hotelDto.Address,
                City = hotelDto.City,
                Country = hotelDto.Country,
                Description = hotelDto.Description,
                Rating = hotelDto.Rating,
                FeaturedImage = hotelDto.FeaturedImage
            };

            var result = await _hotelService.AddHotelAsync(hotel);
            if (!result)
                return BadRequest(new { message = "Failed to add hotel." });

            return Ok(new { message = "Hotel added successfully." });
        }

        [HttpPut("hotels/{id}")]
        public async Task<IActionResult> EditHotel(int id, [FromBody] HotelDto hotelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = new Hotel
            {
                Id = id,
                Name = hotelDto.Name,
                Address = hotelDto.Address,
                City = hotelDto.City,
                Country = hotelDto.Country,
                Description = hotelDto.Description,
                Rating = hotelDto.Rating,
                FeaturedImage = hotelDto.FeaturedImage
            };

            var result = await _hotelService.UpdateHotelAsync(hotel);
            if (!result)
                return BadRequest(new { message = "Failed to update hotel." });

            return Ok(new { message = "Hotel updated successfully." });
        }

        [HttpDelete("hotels/{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var (success, message) = await _hotelService.DeleteHotelAsync(id);
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }

        // ===========================
        //        ROOMS MANAGEMENT
        // ===========================

        [HttpGet("rooms")]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        [HttpPost("rooms")]
        public async Task<IActionResult> CreateRoom([FromBody] RoomDto roomDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room = new Room
            {
                Name = roomDto.Name,
                HotelId = roomDto.HotelId,
                RoomTypeId = roomDto.RoomTypeId,
                PricePerNight = roomDto.PricePerNight,
                IsAvailable = roomDto.IsAvailable
            };

            var result = await _roomService.AddRoomAsync(room);
            if (!result)
                return BadRequest(new { message = "Failed to add room." });

            return Ok(new { message = "Room added successfully." });
        }

        [HttpPut("rooms/{id}")]
        public async Task<IActionResult> EditRoom(int id, [FromBody] RoomDto roomDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room = new Room
            {
                Id = id,
                Name = roomDto.Name,
                HotelId = roomDto.HotelId,
                RoomTypeId = roomDto.RoomTypeId,
                PricePerNight = roomDto.PricePerNight,
                IsAvailable = roomDto.IsAvailable
            };

            var result = await _roomService.UpdateRoomAsync(room);
            if (!result)
                return BadRequest(new { message = "Failed to update room." });

            return Ok(new { message = "Room updated successfully." });
        }

        [HttpDelete("rooms/{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var result = await _roomService.DeleteRoomAsync(id);
            if (!result)
                return BadRequest(new { message = "Failed to delete room." });

            return Ok(new { message = "Room deleted successfully." });
        }
    }
}
