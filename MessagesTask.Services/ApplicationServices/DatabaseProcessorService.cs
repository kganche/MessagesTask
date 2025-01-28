using MessagesTask.Data.Models;
using MessagesTask.Data.Models.Enums;
using MessagesTask.Services.ApplicationServices.Contracts;

namespace MessagesTask.Services.ApplicationServices;

public class DatabaseProcessorService(
    IMessageService messageService,
    IProcessedMessageService processedMessageService
    ) : IMessageProcessorService
{
    private readonly IMessageService _messageService = messageService;
    private readonly IProcessedMessageService _processedMessageService = processedMessageService;

    public async Task ProcessMessageAsync(Message message, CancellationToken cancellationToken = default)
    {
        var processedMessage = new ProcessedMessage
        {
            Id = message.Id,
            Type = message.Type,
            MessageText = message.MessageText,
            ProcessedOn = DateTime.UtcNow
        };

        await _processedMessageService.InsertAsync(processedMessage, cancellationToken);
        await _messageService.UpdateMessageStatusAsync(message, MessageStatus.Completed, cancellationToken);
    }
}
