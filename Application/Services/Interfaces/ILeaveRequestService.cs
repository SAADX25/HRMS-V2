using Application.DTOs.LeaveRequests;
using Domain.Enums;

namespace Application.Services.Interfaces;

public interface ILeaveRequestService
{
    Task<IEnumerable<LeaveRequestDto>> GetAllAsync();
    Task<LeaveRequestDto?> GetByIdAsync(int id);
    Task<LeaveRequestDto> CreateAsync(CreateLeaveRequestDto dto);
    Task<LeaveRequestDto?> UpdateStatusAsync(int id, LeaveStatus newStatus, string? reason, int approvedById);
}
