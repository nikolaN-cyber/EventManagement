using Application.Users.DTOs;
using Domain;
using BCrypt.Net;

namespace Application.Users.Commands
{
    public interface IUserCommandService
    {
        Task<User> RegisterUserAsync(RegisterUserDTO user);
    }
}
