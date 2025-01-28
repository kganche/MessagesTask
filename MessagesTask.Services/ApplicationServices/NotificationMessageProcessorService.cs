using MessagesTask.Data.Models;
using MessagesTask.Data.Models.Enums;
using MessagesTask.Services.ApplicationServices.Contracts;
using Microsoft.Toolkit.Uwp.Notifications;

namespace MessagesTask.Services.ApplicationServices;

public class NotificationMessageProcessorService(IMessageService messageService) : IMessageProcessorService
{
    private readonly IMessageService messageService = messageService;

    public async Task ProcessMessageAsync(Message message, CancellationToken cancellationToken = default)
    {
        new ToastContentBuilder()
            .AddArgument("action", "viewConversation")
            .AddArgument("conversationId", 9813)
            .AddText(message.MessageText)
            .Show();

        await messageService.UpdateMessageStatusAsync(message, MessageStatus.Completed, cancellationToken);
    }
}
