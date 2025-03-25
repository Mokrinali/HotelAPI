using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Models;

namespace HotelBookingApp.Data
{
    public class DataSeeder
    {
        public static async Task SeedData(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)


        {
            // Ensure database is created
            await context.Database.MigrateAsync();

            // Seed Roles
            string[] roleNames = { "Admin", "Manager", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = roleName });

                }
            }

            // Seed Admin User
            var adminEmail = "admin@example.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Administrator",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed User's User
            var userEmail = "user@example.com";
            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                var userUser = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FullName = "System User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(userUser, "User@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(userUser, "User");
                }
            }


            // Seed Sample Hotels
            if (!context.Hotels.Any())
            {
                context.Hotels.AddRange(
                    new Hotel { Name = "Luxury Hotel", Address = "123 Main St", City = "New York", Country = "USA", Description = "A luxury hotel", Rating = 4.5, FeaturedImage = "/images/hotels/luxury.jpg" },
                    new Hotel { Name = "Budget Inn", Address = "456 Elm St", City = "Los Angeles", Country = "USA", Description = "An affordable hotel", Rating = 3.8, FeaturedImage = "/images/hotels/budget.jpg" }
                );

                // Save changes so Room seeding can use these hotels
                await context.SaveChangesAsync();
            }


            // Seed Sample Rooms
            if (!context.Rooms.Any())
            {
                var luxuryHotel = await context.Hotels.FirstOrDefaultAsync(h => h.Name == "Luxury Hotel");
                var budgetHotel = await context.Hotels.FirstOrDefaultAsync(h => h.Name == "Budget Inn");

                // If needed: ensure RoomTypes exist
                var standardType = await context.RoomTypes.FirstOrDefaultAsync() ?? new RoomType { Name = "Standard" };
                if (standardType.Id == 0)
                {
                    context.RoomTypes.Add(standardType);
                    await context.SaveChangesAsync();
                }

                if (luxuryHotel != null && budgetHotel != null)
                {
                    context.Rooms.AddRange(
                        new Room
                        {
                            Name = "King Suite",
                            HotelId = luxuryHotel.Id,
                            RoomTypeId = standardType.Id,
                            PricePerNight = 250.00m,
                            IsAvailable = true,
                            MaximumGuests = 3,
                            ImagePath = "/images/rooms/1.jpg"
                        },
                        new Room
                        {
                            Name = "Deluxe Double",
                            HotelId = luxuryHotel.Id,
                            RoomTypeId = standardType.Id,
                            PricePerNight = 180.00m,
                            IsAvailable = true,
                            MaximumGuests = 2,
                            ImagePath = "/images/rooms/2.jpg"
                        },
                        new Room
                        {
                            Name = "Single Room",
                            HotelId = budgetHotel.Id,
                            RoomTypeId = standardType.Id,
                            PricePerNight = 80.00m,
                            IsAvailable = true,
                            MaximumGuests = 1,
                            ImagePath = "/images/rooms/3.jpg"
                        },
                        new Room
                        {
                            Name = "Economy Room",
                            HotelId = budgetHotel.Id,
                            RoomTypeId = standardType.Id,
                            PricePerNight = 60.00m,
                            IsAvailable = false,
                            MaximumGuests = 2,
                            ImagePath = "/images/rooms/4.jpg"
                        }
                    );

                    await context.SaveChangesAsync();
                }
            }


            await context.SaveChangesAsync();
        }
    }
}
