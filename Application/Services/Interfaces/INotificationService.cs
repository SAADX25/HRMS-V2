using Application.DTOs.Notification;

namespace Application.Services.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<NotificationDto>> GetAllByUserIdAsync(int userId);
    Task<NotificationDto?> GetByIdAsync(int id);
    Task<NotificationDto> CreateAsync(CreateNotificationDto dto);
    Task<bool> MarkAsReadAsync(int id);
    Task<bool> DeleteAsync(int id);
}
