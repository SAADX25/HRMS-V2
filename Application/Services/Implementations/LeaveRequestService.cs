using Application.DTOs.LeaveRequests;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class LeaveRequestService(IUnitOfWork uow, IMapper mapper) : ILeaveRequestService
{
    public async Task<IEnumerable<LeaveRequestDto>> GetAllAsync()
    {
        var requests = await uow.Repository<LeaveRequest>().GetAllQueryable()
            .Include(l => l.Employee)
            .OrderByDescending(l => l.Id)
            .ToListAsync();
        return mapper.Map<IEnumerable<LeaveRequestDto>>(requests);
    }

    public async Task<LeaveRequestDto?> GetByIdAsync(int id)
    {
        var request = await uow.Repository<LeaveRequest>().GetAllQueryable()
            .Include(l => l.Employee)
            .FirstOrDefaultAsync(l => l.Id == id);
            
        return request != null ? mapper.Map<LeaveRequestDto>(request) : null;
    }

    public async Task<LeaveRequestDto> CreateAsync(CreateLeaveRequestDto dto)
    {
        var leaveRequest = mapper.Map<LeaveRequest>(dto);
        leaveRequest.Status = LeaveStatus.Pending;
        
        await uow.Repository<LeaveRequest>().AddAsync(leaveRequest);
        await uow.SaveChangesAsync();
        
        return mapper.Map<LeaveRequestDto>(leaveRequest);
    }

    public async Task<LeaveRequestDto?> UpdateStatusAsync(int id, LeaveStatus newStatus, string? reason, int approvedById)
    {
        var request = await uow.Repository<LeaveRequest>().GetAsync(l => l.Id == id);
        if (request == null) return null;

        request.Status = newStatus;
        if (newStatus == LeaveStatus.Rejected)
        {
            request.RejectionReason = reason;
        }
        
        request.ApprovedBy = approvedById;

        uow.Repository<LeaveRequest>().Update(request);
        await uow.SaveChangesAsync();
        
        return mapper.Map<LeaveRequestDto>(request);
    }
}
