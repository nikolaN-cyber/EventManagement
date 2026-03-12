using Application.Events.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace event_management_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                if (result.Error == "User not recognized") return Unauthorized("Permission denied.");
                return BadRequest(new { Message = result.Error });
            }
            return CreatedAtAction(nameof(CreateEvent), result.Value);
        }

        [Authorize]
        [HttpGet("all-events")]
        public async Task<IActionResult> ListAllEvents()
        {
            var result = await _mediator.Send(new ListAllEventsQuery());
            if (!result.IsSuccess)
            {
                if (result.Error == "User not recognized") return Unauthorized("Permission denied.");
                return BadRequest(new { Message = result.Error });
            }
            return Ok(result.Value);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int Id)
        {
            var result = await _mediator.Send(new DeleteEventQuery(Id));
            if (!result.IsSuccess)
            {
                if (result.Error == "User not recognized") return Unauthorized("Permission denied.");
                return BadRequest(new { Message = result.Error });
            }
            return NoContent();
        }

        [Authorize]
        [HttpPatch("{Id}")]
        public async Task<IActionResult> UpdateEvent([FromRoute] int Id, [FromBody] UpdateEventQuery query)
        {
            query.Id = Id;
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                if (result.Error == "User not recognized") return Unauthorized("Permission denied.");
                return BadRequest(new { Message = result.Error });
            }
            return NoContent();
        }
    }
}
