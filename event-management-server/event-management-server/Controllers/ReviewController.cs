using Application.Events.DTOs;
using Application.Reviews.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace event_management_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                if (result.Error == "User not recognized") return Unauthorized("Permission denied.");
                return BadRequest(new { Message = result.Error });
            }
            return CreatedAtAction(nameof(CreateReview), result.Value);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview([FromRoute] int Id)
        {
            var result = await _mediator.Send(new DeleteReviewQuery(Id));
            if (!result.IsSuccess)
            {
                if (result.Error == "User not recognized") return Unauthorized("Permission denied.");
                return BadRequest(new { Message = result.Error });
            }
            return NoContent();
        }
    }
}
