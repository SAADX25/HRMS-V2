using Application.DTOs.Attendance;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class AttendanceService(IUnitOfWork uow, IMapper mapper) : IAttendanceService
{
    public async Task<IEnumerable<AttendanceDto>> GetAllAsync()
    {
        var attendances = await uow.Repository<Attendance>()
                                   .GetAllQueryable()
                                   .ToListAsync();

        return mapper.Map<IEnumerable<AttendanceDto>>(attendances);
    }

    public async Task<AttendanceDto?> GetByIdAsync(int id)
    {
        var attendance = await uow.Repository<Attendance>()
                                  .GetAllQueryable()
                                  .FirstOrDefaultAsync(a => a.Id == id);

        return attendance is null ? null : mapper.Map<AttendanceDto>(attendance);
    }

    public async Task<IEnumerable<AttendanceDto>> GetByEmployeeIdAsync(int employeeId)
    {
        var attendances = await uow.Repository<Attendance>()
                                   .GetAllQueryable()
                                   .Where(a => a.EmployeeId == employeeId)
                                   .ToListAsync();

        return mapper.Map<IEnumerable<AttendanceDto>>(attendances);
    }

    public async Task<AttendanceDto> CreateAsync(CreateAttendanceDto dto)
    {
        var attendance = mapper.Map<Attendance>(dto);

        await uow.Repository<Attendance>().AddAsync(attendance);
        await uow.SaveChangesAsync();

        return mapper.Map<AttendanceDto>(attendance);
    }

    public async Task<AttendanceDto?> UpdateAsync(int id, UpdateAttendanceDto dto)
    {
        var attendance = await uow.Repository<Attendance>()
                                  .GetAllQueryable()
                                  .FirstOrDefaultAsync(a => a.Id == id);

        if (attendance is null) return null;

        mapper.Map(dto, attendance);
        uow.Repository<Attendance>().Update(attendance);
        await uow.SaveChangesAsync();

        return mapper.Map<AttendanceDto>(attendance);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var attendance = await uow.Repository<Attendance>()
                                  .GetAllQueryable()
                                  .FirstOrDefaultAsync(a => a.Id == id);

        if (attendance is null) return false;

        uow.Repository<Attendance>().Delete(attendance);
        await uow.SaveChangesAsync();
        return true;
    }
}
