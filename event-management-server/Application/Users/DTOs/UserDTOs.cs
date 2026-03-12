namespace Application.Users.DTOs;
using MediatR;

public record RegisterQuery(string Username, string FirstName, string LastName, string Email, string Password, byte[]? Image) : IRequest<RegisterResponse?>;

public record RegisterResponse(int Id, string Username, string FirstName, string LastName, string Email, byte[]? Image);

public record LoginQuery(string Email, string Password) : IRequest<LoginResponse?>;

public record LoginResponse(string Token);
