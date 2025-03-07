using IMS.Application.Dtos.IdentityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.AuthenticationAuthorizationInterface
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(UserDto userDto);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}
