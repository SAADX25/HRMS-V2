using Application.DTOs.Attendance;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var attendances = await _attendanceService.GetAllAsync();
        return Ok(attendances);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var attendance = await _attendanceService.GetByIdAsync(id);
        if (attendance is null)
        {
            return NotFound();
        }

        return Ok(attendance);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployeeId(int employeeId)
    {
        var attendances = await _attendanceService.GetByEmployeeIdAsync(employeeId);
        return Ok(attendances);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAttendanceDto dto)
    {
        var attendance = await _attendanceService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = attendance.Id }, attendance);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateAttendanceDto dto)
    {
        var attendance = await _attendanceService.UpdateAsync(id, dto);
        if (attendance is null)
        {
            return NotFound();
        }

        return Ok(attendance);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _attendanceService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
