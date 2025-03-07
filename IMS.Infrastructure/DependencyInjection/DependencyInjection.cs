using FluentValidation;
using IMS.Application.AuthenticationAuthorizationInterface;
using IMS.Application.Dtos;
using IMS.Application.IServices;
using IMS.Application.Mapper;
using IMS.Application.Services;
using IMS.Application.Validator;
using IMS.Domain.Entities.Identity;
using IMS.Infrastructure.AuthenticationAuthorizationService;
using IMS.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            // Database Context
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Logging
            services.AddSingleton<ILoggerFactory>(provider => LoggerFactory.Create(builder => builder.AddSerilog()));
            // Identity Configuration
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            // Service Registrations
            services.AddAutoMapper();
            services.AddValidations();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IInventoryItemService, InventoryItemService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            //services.AddScoped<ApplicationDbContext>();
            services.AddSingleton<InMemoryCacheService>();

            return services;
        }
        private static void AddValidations(this IServiceCollection services)
        {
            services.AddTransient<IValidator<InventoryItemDto>, InventoryItemValidator>();
            services.AddTransient<IValidator<CategoryDto>, CategoryValidator>();
            services.AddTransient<IValidator<SupplierDto>, SupplierValidator>();

        }
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(InventoryItemProfile).Assembly);
            services.AddAutoMapper(typeof(CategoryProfile).Assembly);
            services.AddAutoMapper(typeof(SupplierProfile).Assembly);
        }
    }
}
