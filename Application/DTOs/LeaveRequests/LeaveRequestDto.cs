using Domain.Enums;

namespace Application.DTOs.LeaveRequests;

public class LeaveRequestDto
{
    public int Id { get; set; }
    public LeaveType LeaveType { get; set; }
    public LeaveStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? RejectionReason { get; set; }
    public int EmployeeId { get; set; }
    public int? ApprovedBy { get; set; }
}
