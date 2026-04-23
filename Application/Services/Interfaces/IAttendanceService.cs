using Application.DTOs.Attendance;

namespace Application.Services.Interfaces;

public interface IAttendanceService
{
    Task<IEnumerable<AttendanceDto>> GetAllAsync();
    Task<AttendanceDto?> GetByIdAsync(int id);
    Task<IEnumerable<AttendanceDto>> GetByEmployeeIdAsync(int employeeId);
    Task<AttendanceDto> CreateAsync(CreateAttendanceDto dto);
    Task<AttendanceDto?> UpdateAsync(int id, UpdateAttendanceDto dto);
    Task<bool> DeleteAsync(int id);
}
