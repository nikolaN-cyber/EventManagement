using Application.Users.Commands;
using Application.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace event_management_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterQuery query)
        {
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return new BadRequestObjectResult(new { Message = "User with that email already exists." });
            }
            return CreatedAtAction(nameof(Register), result);
        }
    }
}
