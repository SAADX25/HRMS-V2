using Application.DTOs.Departments;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentController(IDepartmentService departmentService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await departmentService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await departmentService.GetByIdAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
    {
        var result = await departmentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentDto dto)
    {
        var success = await departmentService.UpdateAsync(id, dto);
        if (!success)
            return NotFound();

        // Get updated entity to return it
        var updated = await departmentService.GetByIdAsync(id);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await departmentService.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
