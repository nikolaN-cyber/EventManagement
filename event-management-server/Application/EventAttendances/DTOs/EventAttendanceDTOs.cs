using Application.Types;
using MediatR;

namespace Application.EventAttendances.DTOs;

public record CreateEventAttendanceQuery(int EventId) : IRequest<Result<EventAttendanceResponse>>;

public record EventAttendanceResponse(int OrganizerId, int EventId, DateTime RegisteredAt);
