using Application.Events.DTOs;
using Application.Types;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Events.Queries
{
    public class ListAllEventsHandler : IRequestHandler<ListAllEventsQuery, Result<List<EventResponse>>>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ListAllEventsHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Result<List<EventResponse>>> Handle(ListAllEventsQuery request, CancellationToken cancellationToken)
        {
            var stringId = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(stringId))
            {
                return Result<List<EventResponse>>.Failure("User not recognized");
            }
            var events = await _context._events
                .Include(e => e.Organizer)
                .OrderBy(e => e.DateAndTime)
                .Select(e => new EventResponse(
                    e.Id,
                    e.Name,
                    e.DateAndTime,
                    e.Location,
                    e.Image,
                    e.Description,
                    e.OrganizerId,
                    e.Organizer.FirstName,
                    e.Organizer.LastName
                    )).ToListAsync(cancellationToken);

            return Result<List<EventResponse>>.Success(events);
        }
    }
}
