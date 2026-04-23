namespace Domain.Entities;

public class Attendance
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public TimeOnly ClockIn { get; set; }
    public TimeOnly? ClockOut { get; set; }  

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
}