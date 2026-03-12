using Application.Types;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EventAttendance.DTOs;

public record CreateEventAttendanceQuery(int EventId) : IRequest<Result<EventAttendanceResponse>>;

public record EventAttendanceResponse(int OrganizerId, int EventId, DateTime RegisteredAt);
