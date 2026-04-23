using Domain.Enums;

namespace Application.DTOs.Notification;

public class CreateNotificationDto
{
    public int UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
}
