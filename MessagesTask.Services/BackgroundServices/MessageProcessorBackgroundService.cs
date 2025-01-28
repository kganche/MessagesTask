using MessagesTask.Data.Models.Enums;
using MessagesTask.Services.ApplicationServices.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MessagesTask.Services.BackgroundServices;

public class MessageProcessorBackgroundService(
    IMessageService messageService,
    [FromKeyedServices(MessageType.File)] IMessageProcessorService fileMessageProcessorServices,
    [FromKeyedServices(MessageType.Database)] IMessageProcessorService databaseMessageProcessorServices,
    [FromKeyedServices(MessageType.Notification)] IMessageProcessorService notificationMessageProcessorServices,
    ILogger<MessageProcessorBackgroundService> logger) : BackgroundService
{
    private readonly IMessageService _messageService = messageService;
    private readonly IMessageProcessorService _fileMessageProcessorServices = fileMessageProcessorServices;
    private readonly IMessageProcessorService _databaseMessageProcessorServices = databaseMessageProcessorServices;
    private readonly IMessageProcessorService _notificationMessageProcessorServices = notificationMessageProcessorServices;
    private readonly ILogger<MessageProcessorBackgroundService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Message Processor Background Service is running.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var newMessages = await _messageService.GetListOfNewMessagesAsync(stoppingToken);

                foreach (var message in newMessages)
                {
                    await _messageService.UpdateMessageStatusAsync(message, MessageStatus.InProgress, stoppingToken);

                    switch (message.Type)
                    {
                        case MessageType.File:
                            await _fileMessageProcessorServices.ProcessMessageAsync(message, stoppingToken);
                            break;
                        case MessageType.Database:
                            await _databaseMessageProcessorServices.ProcessMessageAsync(message, stoppingToken);
                            break;
                        case MessageType.Notification:
                            await _notificationMessageProcessorServices.ProcessMessageAsync(message, stoppingToken);
                            break;
                        default:
                            _logger.LogWarning($"Message type {message.Type} is not supported.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while processing messages.");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
