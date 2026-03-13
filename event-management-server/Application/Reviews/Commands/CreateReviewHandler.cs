using Application.Reviews.DTOs;
using Application.Types;
using Domain;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Reviews.Commands
{
    public class CreateReviewHandler : IRequestHandler<CreateReviewQuery, Result<ReviewResponse>>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateReviewHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Result<ReviewResponse>> Handle(CreateReviewQuery request, CancellationToken cancellationToken)
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Result<ReviewResponse>.Failure("User not recognized");
            }
            var userId = int.Parse(userIdStr);
            var alreadyReviewed = await _context._reviews.AnyAsync(r => r.UserId == userId && r.EventId == request.EventId, cancellationToken);
            if (alreadyReviewed)
            {
                return Result<ReviewResponse>.Failure("User already reviewed this event");
            }
            var newReview = new Review
            {
                UserId = userId,
                EventId = request.EventId,
                Comment = request.Comment,
                Rating = request.Rating
            };

            _context._reviews.Add(newReview);
            await _context.SaveChangesAsync(cancellationToken);

            var reviewResponse = new ReviewResponse
            (
                newReview.Id,
                userId,
                newReview.EventId,
                newReview.Comment,
                newReview.Rating
            );

            return Result<ReviewResponse>.Success(reviewResponse);
        }
    }
}
