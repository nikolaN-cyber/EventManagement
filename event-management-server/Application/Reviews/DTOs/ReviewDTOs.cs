using Application.Types;
using MediatR;

namespace Application.Reviews.DTOs;

public record CreateReviewQuery(int EventId, string Comment, int Rating) : IRequest<Result<ReviewResponse>> ;

public record DeleteReviewQuery(int Id) : IRequest<Result<bool>>;

public record ReviewResponse(int Id, int UserId, int EventId, string Comment, int Rating);

