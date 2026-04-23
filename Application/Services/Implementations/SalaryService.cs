using Application.DTOs.Salary;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class SalaryService(IUnitOfWork uow, IMapper mapper) : ISalaryService
{
    public async Task<IEnumerable<SalaryDto>> GetAllAsync()
    {
        var salaries = await uow.Repository<Salary>()
                                .GetAllQueryable()
                                .ToListAsync();

        return mapper.Map<IEnumerable<SalaryDto>>(salaries);
    }

    public async Task<SalaryDto?> GetByIdAsync(int id)
    {
        var salary = await uow.Repository<Salary>()
                               .GetAllQueryable()
                               .FirstOrDefaultAsync(s => s.Id == id);

        return salary is null ? null : mapper.Map<SalaryDto>(salary);
    }

    public async Task<IEnumerable<SalaryDto>> GetByEmployeeIdAsync(int employeeId)
    {
        var salaries = await uow.Repository<Salary>()
                                .GetAllQueryable()
                                .Where(s => s.EmployeeId == employeeId)
                                .ToListAsync();

        return mapper.Map<IEnumerable<SalaryDto>>(salaries);
    }

    public async Task<SalaryDto> CreateAsync(CreateSalaryDto dto)
    {
        var salary = mapper.Map<Salary>(dto);

        // Calculate Gross and Net amounts if not provided by mapping logic directly.
        // Assuming the mapping simply maps the fields.
        salary.GrossAmount = salary.BaseAmount + salary.Allowances;
        salary.NetAmount = salary.GrossAmount - salary.Deductions;

        await uow.Repository<Salary>().AddAsync(salary);
        await uow.SaveChangesAsync();

        return mapper.Map<SalaryDto>(salary);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var salary = await uow.Repository<Salary>()
                               .GetAllQueryable()
                               .FirstOrDefaultAsync(s => s.Id == id);

        if (salary is null) return false;

        uow.Repository<Salary>().Delete(salary);
        await uow.SaveChangesAsync();
        return true;
    }
}
