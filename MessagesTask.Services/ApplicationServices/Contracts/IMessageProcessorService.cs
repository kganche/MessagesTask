using MessagesTask.Data.Models;

namespace MessagesTask.Services.ApplicationServices.Contracts;

public interface IMessageProcessorService
{
    Task ProcessMessageAsync(Message message, CancellationToken cancellationToken = default);
}
