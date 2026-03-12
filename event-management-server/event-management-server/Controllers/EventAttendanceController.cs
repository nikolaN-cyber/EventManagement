using Application.EventAttendance.DTOs;
using Application.Types;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace event_management_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventAttendanceController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EventAttendanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateEventAttendance([FromBody] CreateEventAttendanceQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                if (result.Error == "User not recognized") return Unauthorized("Permission denied.");
                return BadRequest(new { Message = result.Error });
            }
            return CreatedAtAction(nameof(CreateEventAttendance), result.Value);
        }
    }
}
