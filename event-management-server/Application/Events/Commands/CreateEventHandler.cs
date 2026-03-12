using Application.Events.DTOs;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Application.Types;
using Domain;

namespace Application.Events.Commands
{
    public class CreateEventHandler : IRequestHandler<CreateEventQuery, Result<EventResponse>> 
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateEventHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<EventResponse>> Handle(CreateEventQuery request, CancellationToken cancellationToken)
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Result<EventResponse>.Failure("User not recognized");
            }
            var userId = int.Parse(userIdStr);

            var existingEvent = await _context._events.AnyAsync(e =>
                e.Name.ToLower() == request.Name.ToLower() &&
                e.DateAndTime == request.DateAndTime &&
                e.Location.ToLower() == request.Location.ToLower()
            );

            if (existingEvent)
            {
                return Result<EventResponse>.Failure("Event already exists");
            }

            var newEvent = new Event
            {
                Name = request.Name,
                DateAndTime = request.DateAndTime,
                Location = request.Location,
                Description = request.Description,
                OrganizerId = userId
            };

            _context._events.Add(newEvent);
            await _context.SaveChangesAsync(cancellationToken);

            var newResponse = new EventResponse
            (
                newEvent.Id,
                newEvent.Name,
                newEvent.DateAndTime,
                newEvent.Location,
                newEvent.Image,
                newEvent.Description,
                newEvent.OrganizerId
            );

            return Result<EventResponse>.Success(newResponse);
        }
    }
}
