using Application.DTOs.LeaveRequests;
using Application.Services.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LeaveRequestController(ILeaveRequestService leaveRequestService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await leaveRequestService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await leaveRequestService.GetByIdAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLeaveRequestDto dto)
    {
        var result = await leaveRequestService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateLeaveRequestStatusDto dto)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int.TryParse(userIdString, out int adminId);

        var result = await leaveRequestService.UpdateStatusAsync(id, dto.Status, dto.RejectionReason, adminId);
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}

public class UpdateLeaveRequestStatusDto
{
    public LeaveStatus Status { get; set; }
    public string? RejectionReason { get; set; }
}
