using Domain.Enums;

namespace Domain.Entities;

public class LeaveRequest
{
    public int Id { get; set; }
    public LeaveType LeaveType { get; set; }      //  enum 
    public LeaveStatus Status { get; set; }        //  enum 
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? RejectionReason { get; set; }

    // Relationships
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public int? ApprovedBy { get; set; }          
    public User? ApprovedByUser { get; set; }     
}