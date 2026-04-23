using Application.DTOs.Notification;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAllByUser(int userId)
    {
        var notifications = await _notificationService.GetAllByUserIdAsync(userId);
        return Ok(notifications);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var notification = await _notificationService.GetByIdAsync(id);
        if (notification is null) return NotFound();

        return Ok(notification);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateNotificationDto dto)
    {
        var notification = await _notificationService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = notification.Id }, notification);
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var result = await _notificationService.MarkAsReadAsync(id);
        if (!result) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _notificationService.DeleteAsync(id);
        if (!result) return NotFound();

        return NoContent();
    }
}
