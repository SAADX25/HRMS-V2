namespace Application.DTOs.Attendance;

public class CreateAttendanceDto
{
    public DateTime Date { get; set; }
    public TimeOnly ClockIn { get; set; }
    public int EmployeeId { get; set; }
}
