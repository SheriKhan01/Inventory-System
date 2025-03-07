using IMS.Application.AuthenticationAuthorizationInterface;
using IMS.Application.Dtos.IdentityDto;
using IMS.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.AuthenticationAuthorizationService
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            var roles = _roleManager.Roles.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name
            });

            return await Task.FromResult(roles.ToList());
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            var result = await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });
            return result.Succeeded;
        }
    }
}
