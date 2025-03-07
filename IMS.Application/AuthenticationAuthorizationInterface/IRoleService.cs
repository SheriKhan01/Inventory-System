using IMS.Application.Dtos.IdentityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.AuthenticationAuthorizationInterface
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<bool> CreateRoleAsync(string roleName);
    }
}
