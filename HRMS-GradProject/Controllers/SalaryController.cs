using Application.DTOs.Salary;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalaryController : ControllerBase
{
    private readonly ISalaryService _salaryService;

    public SalaryController(ISalaryService salaryService)
    {
        _salaryService = salaryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var salaries = await _salaryService.GetAllAsync();
        return Ok(salaries);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var salary = await _salaryService.GetByIdAsync(id);
        if (salary is null)
        {
            return NotFound();
        }

        return Ok(salary);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployeeId(int employeeId)
    {
        var salaries = await _salaryService.GetByEmployeeIdAsync(employeeId);
        return Ok(salaries);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSalaryDto dto)
    {
        var salary = await _salaryService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = salary.Id }, salary);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _salaryService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
