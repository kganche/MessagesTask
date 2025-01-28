using MessagesTask.Data.Models;
using MessagesTask.Data.Models.Enums;
using MessagesTask.Services.ApplicationServices.Contracts;

namespace MessagesTask.Services.ApplicationServices;

public class FileMessageProcessorService(IMessageService messageService) : IMessageProcessorService
{
    private readonly IMessageService _messageService = messageService;

    public async Task ProcessMessageAsync(Message message, CancellationToken cancellationToken = default)
    {
        string fileName = $"{message.Id}.txt";

        await File.WriteAllTextAsync(fileName, message.MessageText, cancellationToken);

        await _messageService.UpdateMessageStatusAsync(message, MessageStatus.Completed, cancellationToken);
    }
}
