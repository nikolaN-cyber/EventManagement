using Application.Events.DTOs;
using Application.Types;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Events.Commands
{
    public class UpdateEventHandler : IRequestHandler<UpdateEventQuery, Result<bool>>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateEventHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Result<bool>> Handle(UpdateEventQuery request, CancellationToken cancellationToken)
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return Result<bool>.Failure("User not recognized");
            var userId = int.Parse(userIdStr);

            var eventToModify = await _context._events.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (eventToModify == null)
            {
                return Result<bool>.Failure("Event with this ID does not exist");
            }

            if (eventToModify.OrganizerId != userId)
            {
                return Result<bool>.Failure("You can only modify your own events");
            }
            eventToModify.Name = request.Name;
            eventToModify.Location = request.Location;
            eventToModify.DateAndTime = request.DateAndTime;
            eventToModify.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
