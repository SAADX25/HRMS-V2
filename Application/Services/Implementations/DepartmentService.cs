using Application.DTOs.Departments;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class DepartmentService(IUnitOfWork uow, IMapper mapper) : IDepartmentService
{
    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await uow.Repository<Department>().GetAllAsync();
        return mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        var department = await uow.Repository<Department>().GetAsync(d => d.Id == id);
        return department != null ? mapper.Map<DepartmentDto>(department) : null;
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        var department = mapper.Map<Department>(dto);
        await uow.Repository<Department>().AddAsync(department);
        await uow.SaveChangesAsync();
        return mapper.Map<DepartmentDto>(department);
    }

    public async Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto)
    {
        var department = await uow.Repository<Department>().GetAsync(d => d.Id == id);
        if (department == null) return false;

        mapper.Map(dto, department);
        uow.Repository<Department>().Update(department);
        await uow.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department = await uow.Repository<Department>().GetAsync(d => d.Id == id);
        if (department == null) return false;

        // Ensure we don't delete if there are employees. (Ideally, the repository or DB handles this constraint)
        uow.Repository<Department>().Delete(department);
        await uow.SaveChangesAsync();
        return true;
    }
}
