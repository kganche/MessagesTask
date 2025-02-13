using MessagesTask.Data.Models;
using MessagesTask.Data.Models.Enums;
using MessagesTask.Services.ApplicationServices.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace MessagesTask.Services.ApplicationServices;

public class StrategyResolver(
	[FromKeyedServices(MessageType.File)] IMessageProcessorService fileMessageProcessorServices,
	[FromKeyedServices(MessageType.Database)] IMessageProcessorService databaseMessageProcessorServices,
	[FromKeyedServices(MessageType.Notification)] IMessageProcessorService notificationMessageProcessorServices) : IStrategyResolver
{
	public async Task ResolveStrategyAsync(Message message, CancellationToken cancellationToken = default)
	{
		switch (message.Type)
		{
			case MessageType.File:
				await fileMessageProcessorServices.ProcessMessageAsync(message, cancellationToken);
				break;
			case MessageType.Database:
				await databaseMessageProcessorServices.ProcessMessageAsync(message, cancellationToken);
				break;
			case MessageType.Notification:
				await notificationMessageProcessorServices.ProcessMessageAsync(message, cancellationToken);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(message.Type), message.Type, "Message type not supported.");
		}
	}
}