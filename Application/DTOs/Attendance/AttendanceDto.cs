namespace Application.DTOs.Attendance;

public class AttendanceDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public TimeOnly ClockIn { get; set; }
    public TimeOnly? ClockOut { get; set; }  
    public int EmployeeId { get; set; }
}
