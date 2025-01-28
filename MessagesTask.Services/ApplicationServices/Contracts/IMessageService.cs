using MessagesTask.Data.Models;
using MessagesTask.Data.Models.Enums;

namespace MessagesTask.Services.ApplicationServices.Contracts;

public interface IMessageService
{
    Task<IEnumerable<Message>> GetListOfNewMessagesAsync(CancellationToken cancellationToken = default);

    Task UpdateMessageStatusAsync(Message message, MessageStatus status, CancellationToken cancellationToken = default);
}
