using Application.DTOs.Notification;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class NotificationService(IUnitOfWork uow, IMapper mapper) : INotificationService
{
    public async Task<IEnumerable<NotificationDto>> GetAllByUserIdAsync(int userId)
    {
        var notifications = await uow.Repository<Notification>()
                                     .GetAllQueryable()
                                     .Where(n => n.UserId == userId)
                                     .OrderByDescending(n => n.CreatedAt)
                                     .ToListAsync();

        return mapper.Map<IEnumerable<NotificationDto>>(notifications);
    }

    public async Task<NotificationDto?> GetByIdAsync(int id)
    {
        var notification = await uow.Repository<Notification>()
                                     .GetAllQueryable()
                                     .FirstOrDefaultAsync(n => n.Id == id);

        return notification is null ? null : mapper.Map<NotificationDto>(notification);
    }

    public async Task<NotificationDto> CreateAsync(CreateNotificationDto dto)
    {
        var notification = mapper.Map<Notification>(dto);
        notification.CreatedAt = DateTime.UtcNow;
        notification.IsRead = false;

        await uow.Repository<Notification>().AddAsync(notification);
        await uow.SaveChangesAsync();

        return mapper.Map<NotificationDto>(notification);
    }

    public async Task<bool> MarkAsReadAsync(int id)
    {
        var notification = await uow.Repository<Notification>()
                                     .GetAllQueryable()
                                     .FirstOrDefaultAsync(n => n.Id == id);

        if (notification is null) return false;

        notification.IsRead = true;
        uow.Repository<Notification>().Update(notification);
        await uow.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var notification = await uow.Repository<Notification>()
                                     .GetAllQueryable()
                                     .FirstOrDefaultAsync(n => n.Id == id);

        if (notification is null) return false;

        uow.Repository<Notification>().Delete(notification);
        await uow.SaveChangesAsync();
        
        return true;
    }
}
