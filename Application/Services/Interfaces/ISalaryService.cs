using Application.DTOs.Salary;

namespace Application.Services.Interfaces;

public interface ISalaryService
{
    Task<IEnumerable<SalaryDto>> GetAllAsync();
    Task<SalaryDto?> GetByIdAsync(int id);
    Task<IEnumerable<SalaryDto>> GetByEmployeeIdAsync(int employeeId);
    Task<SalaryDto> CreateAsync(CreateSalaryDto dto);
    Task<bool> DeleteAsync(int id);
}
