using Application.Users.Commands;
using Application.Users.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace event_management_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserCommandService _userCommandService;
        public UserController(IUserCommandService userCommandService)
        {
            _userCommandService = userCommandService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO user)
        {
            try
            {
                var registeredUser = await _userCommandService.RegisterUserAsync(user);
                return Ok(new
                {
                    Id = registeredUser.Id,
                    Username = registeredUser.Username,
                    FirstName = registeredUser.FirstName,
                    LastName = registeredUser.LastName,
                    Email = registeredUser.Email,
                    Message = "Korisnik uspešno registrovan!"
                });
            } catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
    }
}
