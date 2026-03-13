using Application.Reviews.DTOs;
using Application.Types;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Reviews.Commands
{
    public class DeleteReviewHandler : IRequestHandler<DeleteReviewQuery, Result<bool>>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteReviewHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Result<bool>> Handle(DeleteReviewQuery request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst("id").Value;
            if (string.IsNullOrEmpty(userIdString))
            {
                return Result<bool>.Failure("User not recognized");
            }
            var userId = int.Parse(userIdString);

            var reviewToDelete = await _context._reviews.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (reviewToDelete == null)
            {
                return Result<bool>.Failure("Review with this Id does not exist");
            }

            if (userId != reviewToDelete.UserId)
            {
                return Result<bool>.Failure("You can only delete your own reviews");
            }

            _context._reviews.Remove(reviewToDelete);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
