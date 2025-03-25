using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HotelBookingApp.Services;
using HotelBookingApp.DTOs;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using HotelBookingApp.Models;
using System.IO;

namespace HotelBookingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService;
        private readonly IRoomTypeService _roomTypeService;

        public RoomController(IRoomService roomService, IHotelService hotelService, IRoomTypeService roomTypeService)
        {
            _roomService = roomService;
            _hotelService = hotelService;
            _roomTypeService = roomTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms([FromQuery] int? hotelId, [FromQuery] decimal? minPrice,
                                                  [FromQuery] decimal? maxPrice, [FromQuery] int? roomTypeId,
                                                  [FromQuery] int? guests)
        {
            var rooms = hotelId.HasValue
                ? await _roomService.GetRoomsByHotelIdAsync(hotelId.Value)
                : await _roomService.GetAllRoomsAsync();

            if (minPrice.HasValue)
                rooms = rooms.Where(r => r.PricePerNight >= minPrice.Value).ToList();
            if (maxPrice.HasValue)
                rooms = rooms.Where(r => r.PricePerNight <= maxPrice.Value).ToList();
            if (roomTypeId.HasValue)
                rooms = rooms.Where(r => r.RoomTypeId == roomTypeId.Value).ToList();
            if (guests.HasValue)
                rooms = rooms.Where(r => r.MaximumGuests >= guests.Value).ToList();

            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
                return NotFound("Room not found.");
            return Ok(room);
        }

        [Authorize]
        [HttpPost("Book/{id}")]
        public async Task<IActionResult> BookRoom(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null || !room.IsAvailable)
                return BadRequest(new { message = "Room is not available for booking." });

            // Simulate booking logic
            return Ok(new { message = $"Room '{room.Name}' successfully booked." });
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromForm] RoomDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string imagePath = model.ImagePath;
            if (model.ImageFile != null)
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/rooms");
                Directory.CreateDirectory(uploadPath);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                string fullPath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                imagePath = "/images/rooms/" + fileName;
            }

            var room = new Room
            {
                Name = model.Name,
                HotelId = model.HotelId,
                RoomTypeId = model.RoomTypeId,
                PricePerNight = model.PricePerNight,
                MaximumGuests = model.MaximumGuests,
                IsAvailable = model.IsAvailable,
                ImagePath = imagePath
            };

            var result = await _roomService.AddRoomAsync(room);
            if (!result)
                return BadRequest(new { message = "Failed to create room." });

            return Ok(new { message = "Room created successfully." });
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromForm] RoomDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var validHotel = await _hotelService.GetHotelByIdAsync(model.HotelId);
            if (validHotel == null)
                return BadRequest(new { message = "Invalid hotel selected." });

            var room = await _roomService.GetRoomEntityByIdAsync(id);
            if (room == null)
                return NotFound();

            string imagePath = model.ImagePath;
            if (model.ImageFile != null)
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/rooms");
                Directory.CreateDirectory(uploadPath);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                string fullPath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                imagePath = "/images/rooms/" + fileName;
            }

            room.Name = model.Name;
            room.PricePerNight = model.PricePerNight;
            room.RoomTypeId = model.RoomTypeId;
            room.IsAvailable = model.IsAvailable;
            room.MaximumGuests = model.MaximumGuests;
            room.HotelId = model.HotelId;
            room.ImagePath = imagePath;

            var success = await _roomService.UpdateRoomAsync(room);
            if (!success)
                return BadRequest(new { message = "Failed to update room." });

            return Ok(new { message = "Room updated successfully." });
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var success = await _roomService.DeleteRoomAsync(id);
            if (!success)
                return BadRequest(new { message = "Failed to delete room." });

            return Ok(new { message = "Room deleted successfully." });
        }
    }
}
