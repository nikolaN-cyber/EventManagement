using Application.Users.DTOs;
using Domain;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands
{
    public class RegisterHandler : IRequestHandler<RegisterQuery, RegisterResponse?>
    {
        private readonly AppDbContext _context;

        public RegisterHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RegisterResponse?> Handle(RegisterQuery request, CancellationToken cancellationToken)
        {
            var existingUser = await _context._users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (existingUser)
            {
                return null;
            }

            string hashedPass = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = new User
            {
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                HashedPassword = hashedPass,
                ProfileImage = request.Image
            };

            _context._users.Add(newUser);
            await _context.SaveChangesAsync(cancellationToken);
            return new RegisterResponse
            (
                newUser.Id,
                newUser.Username,
                newUser.FirstName,
                newUser.LastName,
                newUser.Email,
                newUser.ProfileImage
            );
        }
    }
}
