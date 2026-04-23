using Application.DTOs.Employee;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class EmployeeService(IUnitOfWork uow, IMapper mapper) : IEmployeeService
{
    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await uow.Repository<Employee>()
                                 .GetAllQueryable()
                                 .Include(e => e.Department)
                                 .ToListAsync();

        return mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var employee = await uow.Repository<Employee>()
                                .GetAllQueryable()
                                .Include(e => e.Department)
                                .FirstOrDefaultAsync(e => e.Id == id);

        return employee is null ? null : mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        var isUnique = await uow.Repository<Employee>()
                                .GetAllQueryable()
                                .AnyAsync(e => e.Email == dto.Email);

        if (isUnique)
            throw new Exception("Previously used email");

        var employee = mapper.Map<Employee>(dto);
        employee.IsActive = true;

        await uow.Repository<Employee>().AddAsync(employee);
        await uow.SaveChangesAsync();

        return mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto?> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var employee = await uow.Repository<Employee>()
                                .GetAllQueryable()
                                .FirstOrDefaultAsync(e => e.Id == id);

        if (employee is null) return null;

        mapper.Map(dto, employee);
        uow.Repository<Employee>().Update(employee);
        await uow.SaveChangesAsync();

        return mapper.Map<EmployeeDto>(employee);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await uow.Repository<Employee>()
                                .GetAllQueryable()
                                .FirstOrDefaultAsync(e => e.Id == id);

        if (employee is null) return false;

        uow.Repository<Employee>().Delete(employee);
        await uow.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(int departmentId)
    {
        var employees = await uow.Repository<Employee>()
                                 .GetAllQueryable()
                                 .Where(e => e.DepartmentId == departmentId)
                                 .Include(e => e.Department)
                                 .ToListAsync();

        return mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
}