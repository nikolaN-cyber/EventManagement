using Application.EventAttendances.DTOs;
using Application.Types;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.EventAttendances.Commands
{
    public class CreateEventAttendanceHandler : IRequestHandler<CreateEventAttendanceQuery, Result<EventAttendanceResponse>>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateEventAttendanceHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Result<EventAttendanceResponse>> Handle(CreateEventAttendanceQuery request, CancellationToken cancellationToken)
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Result<EventAttendanceResponse>.Failure("User not recognized");
            }
            var userId = int.Parse(userIdStr);

            var alreadyRegistered = await _context._eventAttendancies.AnyAsync(ea => ea.OrganizerId == userId && ea.EventId == request.EventId);
            if (alreadyRegistered)
            {
                return Result<EventAttendanceResponse>.Failure("User is already registered for this event");
            }

            var attendance = new Domain.EventAttendance
            {
                OrganizerId = userId,
                EventId = request.EventId,
            };

            _context._eventAttendancies.Add(attendance);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<EventAttendanceResponse>.Success(new EventAttendanceResponse(attendance.OrganizerId, attendance.EventId, attendance.RegisteredAt));
        }
    }
}
