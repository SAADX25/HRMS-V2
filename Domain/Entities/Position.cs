namespace Domain.Entities;

public class Position
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;  // ENG , Dev 
    public decimal SalaryMin { get; set; }              
    public decimal SalaryMax { get; set; }
    public int DepartmentId { get; set; }

    public Department Department { get; set; } = null!;
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}