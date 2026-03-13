using MediatR;
using Application.Types;

namespace Application.Events.DTOs;

public record CreateEventQuery(string Name, DateTime DateAndTime, string Location, byte[]? Image, string Description) : IRequest<Result<bool>>;

public record ListAllEventsQuery() : IRequest<Result<List<EventResponse>>>;

public record DeleteEventQuery(int Id) : IRequest<Result<bool>>;

public record UpdateEventQuery(int Id, string Name, DateTime DateAndTime, string Location, string Description) : IRequest<Result<bool>> { public int Id { get; set; } };

public record EventResponse(int Id, string Name, DateTime DateAndTime, string Location, byte[]? Image, string Description, int OrganizerId, string OrganizerFirstName, string OrganizerLastName);
