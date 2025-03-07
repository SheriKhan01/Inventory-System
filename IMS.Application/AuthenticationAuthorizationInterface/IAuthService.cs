
using IMS.Application.Dtos.IdentityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.AuthenticationAuthorizationInterface
{
    public interface IAuthService
    {
        Task<string> GenerateJwtToken(UserDto userDto);
        Task<UserDto> RegisterUser(string email, string password, string firstName, string lastName);
        Task<UserDto> LoginUser(string email, string password);
    }
}
