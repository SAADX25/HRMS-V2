using Application.DTOs.Dashboard;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class DashboardService(IUnitOfWork uow) : IDashboardService
{
    public async Task<DashboardDto> GetDashboardStatsAsync()
    {
        var totalEmployees = await uow.Repository<Employee>().GetAllQueryable().CountAsync();
        
        var totalDepartments = await uow.Repository<Department>().GetAllQueryable().CountAsync();
        
        var pendingLeaveRequests = await uow.Repository<LeaveRequest>()
                                            .GetAllQueryable()
                                            .CountAsync(lr => lr.Status == LeaveStatus.Pending);
                                            
        var today = DateTime.UtcNow.Date;
        var todayPresentEmployees = await uow.Repository<Attendance>()
                                             .GetAllQueryable()
                                             .CountAsync(a => a.Date.Date == today);

        return new DashboardDto
        {
            TotalEmployees = totalEmployees,
            TotalDepartments = totalDepartments,
            PendingLeaveRequests = pendingLeaveRequests,
            TodayPresentEmployees = todayPresentEmployees
        };
    }
}
