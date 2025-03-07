using IMS.Application.AuthenticationAuthorizationInterface;
using IMS.Application.Dtos.IdentityDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AccountController(IAuthService authService, IUserService userService, IRoleService roleService)
        {
            _authService = authService;
            _userService = userService;
            _roleService = roleService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _authService.RegisterUser(registerDto.Email, registerDto.Password, registerDto.FirstName, registerDto.LastName);
            if (user == null)
                return BadRequest("User registration failed.");

            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _authService.LoginUser(loginDto.Email, loginDto.Password);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            var token = await _authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }
        [Authorize]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("role/create")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var result = await _roleService.CreateRoleAsync(roleName);
            if (!result)
                return BadRequest("Role creation failed.");

            return Ok(new { Message = "Role created successfully." });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }
    }
}
