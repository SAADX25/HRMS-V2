using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.LeaveRequests;

public class CreateLeaveRequestDto
{
    [Required]
    public LeaveType LeaveType { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public int EmployeeId { get; set; }
}
