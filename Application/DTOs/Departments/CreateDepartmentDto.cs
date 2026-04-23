using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Departments;

public class CreateDepartmentDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Location { get; set; } = string.Empty;
}
