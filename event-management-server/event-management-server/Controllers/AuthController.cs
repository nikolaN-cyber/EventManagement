using Application.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace event_management_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Loginuser([FromBody] LoginQuery query)
        {
            var response = await _mediator.Send(query);
            if (response == null)
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }
            return Ok(response);
        }
    }
}
