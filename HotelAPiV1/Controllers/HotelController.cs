using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Services;
using HotelBookingApp.Models;
using HotelBookingApp.DTOs;
using System.IO;

namespace HotelBookingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var hotels = await _hotelService.GetAllHotelsAsync();

            var hotelDtos = hotels.Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Address = h.Address,
                City = h.City,
                Country = h.Country,
                Description = h.Description,
                Rating = h.Rating,
                FeaturedImage = h.FeaturedImage
            }).ToList();

            return Ok(hotelDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
                return NotFound();

            var hotelDto = new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                City = hotel.City,
                Country = hotel.Country,
                Description = hotel.Description,
                Rating = hotel.Rating,
                FeaturedImage = hotel.FeaturedImage,
                Rooms = hotel.Rooms?.Select(r => new RoomDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    ImagePath = r.ImagePath
                }).ToList() ?? new List<RoomDto>()
            };

            return Ok(hotelDto);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] HotelDto model)
        {
            ModelState.Remove(nameof(model.FeaturedImage));
            model.FeaturedImage = "/images/no-image.png";

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/hotels");
                Directory.CreateDirectory(uploadPath);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                model.FeaturedImage = "/images/hotels/" + fileName;
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = new Hotel
            {
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                Description = model.Description,
                Rating = model.Rating,
                FeaturedImage = model.FeaturedImage
            };

            var result = await _hotelService.AddHotelAsync(hotel);
            if (!result)
                return BadRequest(new { message = "Failed to create hotel." });

            return Ok(new { message = "Hotel created successfully." });
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] HotelDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string imagePath = model.FeaturedImage;

            if (model.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/hotels");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                imagePath = "/images/hotels/" + uniqueFileName;
            }

            var hotel = new Hotel
            {
                Id = id,
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                Description = model.Description,
                Rating = model.Rating,
                FeaturedImage = imagePath
            };

            await _hotelService.UpdateHotelAsync(hotel);
            return Ok(new { message = "Hotel updated successfully." });
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _hotelService.DeleteHotelAsync(id);
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }
    }
}
