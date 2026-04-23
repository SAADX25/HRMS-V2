// Aother : Abedalqader Alfaqeeh
// last Edit : 2026/04/12
  

using Application.DTOs.Employee;

namespace Application.Services.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
    Task<EmployeeDto?> UpdateAsync(int id, UpdateEmployeeDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(int departmentId);
}