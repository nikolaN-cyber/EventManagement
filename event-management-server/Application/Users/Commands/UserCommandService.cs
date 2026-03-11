using Application.Users.DTOs;
using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands
{
    public class UserCommandService : IUserCommandService
    {
        private readonly AppDbContext _context;
        public UserCommandService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User> RegisterUserAsync(RegisterUserDTO user)
        {
            var exists = await _context._users.AnyAsync(u => u.Email == user.Email);
            if (exists)
            {
                throw new Exception("User with email: " + user.Email + ", already exists.");
            }
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);

            var newUser = new User
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                HashedPassword = hashedPassword,
                ProfileImage = user.Image
            };

            _context._users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }
    }
}
