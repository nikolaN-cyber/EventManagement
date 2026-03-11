namespace Application.Users.DTOs;

public record RegisterUserDTO(string Username, string FirstName, string LastName, string Email, string Password, byte[]? Image);
