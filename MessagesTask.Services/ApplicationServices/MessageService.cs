using MessagesTask.Data;
using MessagesTask.Data.Models;
using MessagesTask.Data.Models.Enums;
using MessagesTask.Services.ApplicationServices.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MessagesTask.Services.ApplicationServices;

public class MessageService(IServiceScopeFactory serviceScopeFactory) : IMessageService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    public async Task<IEnumerable<Message>> GetListOfNewMessagesAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetRequiredService<MessagesDbContext>();

        return await _dbContext.Messages
            .Where(m => m.Status == MessageStatus.New)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateMessageStatusAsync(Message message, MessageStatus status, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetRequiredService<MessagesDbContext>();

        message.Status = status;
        _dbContext.Messages.Update(message);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
