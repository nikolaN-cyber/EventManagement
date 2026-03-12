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
    public class DeleteEventHandler : IRequestHandler<DeleteEventQuery, Result<bool>>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteEventHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Result<bool>> Handle(DeleteEventQuery request, CancellationToken cancellationToken)
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return Result<bool>.Failure("User not recognized");
            var userId = int.Parse(userIdStr);

            var eventToDelete = await _context._events.FirstOrDefaultAsync(e => e.Id == request.Id);
            if (eventToDelete==null)
            {
                return Result<bool>.Failure("Event with this ID does not exist");
            }

            if (eventToDelete.OrganizerId != userId)
            {
                return Result<bool>.Failure("You can only delete your own events");
            }

            _context._events.Remove(eventToDelete);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
