using MessagesTask.Data.Models;

namespace MessagesTask.Services.ApplicationServices.Contracts;

public interface IProcessedMessageService
{
    Task InsertAsync(ProcessedMessage processedMessage, CancellationToken cancellationToken = default);
}
