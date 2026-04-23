using Domain.Entities;

namespace Domain.Interfaces;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    // Add any additional methods specific to Employee repository here
    Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId);

    // Method to check if an email is unique (not already used by another employee)
    Task<bool> IsEmailUniqueAsync(string email);
}