using IMS.Domain.Entities;
using IMS.Domain.Entities.Identity;
using IMS.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.SeedData
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedData(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            // Ensure database is created
            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync();
            }

           
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
            await SeedCategories(context);
            await SeedSuppliers(context);
            await SeedInventoryItems(context);

        }

        private static async Task SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            var roles = new List<string> { "Admin", "Manager", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role });
                }
            }
        }

        private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            string adminEmail = "admin@ims.com";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin", // ✅ Ensure FirstName is set
                    LastName = "User",   // ✅ Ensure LastName is set
                    PhoneNumber = "1234567890",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine("✅ Admin user created successfully.");
                }
                else
                {
                    Console.WriteLine("❌ Failed to create Admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
        private static async Task SeedCategories(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Id = Guid.NewGuid(), Name = "Electronics", Description = "All electronic items" },
                    new Category { Id = Guid.NewGuid(), Name = "Furniture", Description = "Furniture items" },
                    new Category { Id = Guid.NewGuid(), Name = "Stationery", Description = "Office stationery" }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedSuppliers(ApplicationDbContext context)
        {
            if (!context.Suppliers.Any())
            {
                var suppliers = new List<Supplier>
                {
                    new Supplier { Id = Guid.NewGuid(), Name = "Tech Supplier", ContactPerson = "John Doe", PhoneNumber = "1234567890" },
                    new Supplier { Id = Guid.NewGuid(), Name = "Office Supplies", ContactPerson = "Jane Smith", PhoneNumber = "0987654321" }
                };

                await context.Suppliers.AddRangeAsync(suppliers);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedInventoryItems(ApplicationDbContext context)
        {
            if (!context.InventoryItems.Any())
            {
                var inventoryItems = new List<InventoryItem>
                {
                    new InventoryItem
                    {
                        Id = Guid.NewGuid(),
                        Name = "Laptop",
                        Description = "Gaming Laptop",
                        Quantity = 10,
                        Price = 1000.00m,
                        CategoryId = context.Categories.First(c => c.Name == "Electronics").Id,
                        SupplierId = context.Suppliers.First(s => s.Name == "Tech Supplier").Id
                    },
                    new InventoryItem
                    {
                        Id = Guid.NewGuid(),
                        Name = "Office Chair",
                        Description = "Ergonomic Chair",
                        Quantity = 20,
                        Price = 150.00m,
                        CategoryId = context.Categories.First(c => c.Name == "Furniture").Id,
                        SupplierId = context.Suppliers.First(s => s.Name == "Office Supplies").Id
                    }
                };

                await context.InventoryItems.AddRangeAsync(inventoryItems);
                await context.SaveChangesAsync();
            }
        }

    }
}
