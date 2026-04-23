using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Auth;

public class RegisterDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Employee;

    public int? EmployeeId { get; set; }
}