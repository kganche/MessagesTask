using MessagesTask.Data.Models;

namespace MessagesTask.Services.ApplicationServices.Contracts;

public interface IStrategyResolver
{
    Task ResolveStrategyAsync(Message message, CancellationToken cancellationToken = default);
}