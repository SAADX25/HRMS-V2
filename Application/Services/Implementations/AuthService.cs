using Application.DTOs.Auth;
using Application.Interfaces;
using BCrypt.Net;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class AuthService(IUnitOfWork uow, IJwtService jwtService) : IAuthService
    {
        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await uow.Repository<User>()
                            .GetAllQueryable()
                            .Include(u => u.Employee)
                            .FirstOrDefaultAsync(u => u.Email == dto.Email);


            if(user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return null;
            }

            return new AuthResponseDto
            {
                Token = jwtService.GenerateToken(user),
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString(),
                ExpiresAt = jwtService.GetExpiration()
            };

        }
        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            var exists = await uow.Repository<User>()
                                  .GetAllQueryable()
                                  .AnyAsync(u => u.Email == dto.Email);
            if (exists) return false;

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role.ToString(),
                EmployeeId = dto.EmployeeId,
                CreatedAt = DateTime.UtcNow
            };

            await uow.Repository<User>().AddAsync(user);
            await uow.SaveChangesAsync();
            return true;
        }


    }
}
