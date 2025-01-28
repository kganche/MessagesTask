using MessagesTask.Data;
using MessagesTask.Data.Models;
using MessagesTask.Services.ApplicationServices.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace MessagesTask.Services.ApplicationServices;

public class ProcessedMessageService : IProcessedMessageService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ProcessedMessageService(IServiceScopeFactory serviceScopeFactory) => _serviceScopeFactory = serviceScopeFactory;

    public async Task InsertAsync(ProcessedMessage processedMessage, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetRequiredService<MessagesDbContext>();

        await _dbContext.ProcessedMessages.AddAsync(processedMessage, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
