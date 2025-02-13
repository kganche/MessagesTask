using MessagesTask.Data.Models.Enums;
using MessagesTask.Services.ApplicationServices.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MessagesTask.Data.Models;

namespace MessagesTask.Services.BackgroundServices
{
    public class MessageProcessorBackgroundService(
        IMessageService messageService,
        IStrategyResolver strategyResolver,
        ILogger<MessageProcessorBackgroundService> logger) : BackgroundService
    {
        private readonly SemaphoreSlim _semaphore = new(3);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Message Processor Background Service is running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var newMessages = await messageService.GetListOfNewMessagesAsync(stoppingToken);
                var tasks = new List<Task>();

                foreach (var message in newMessages)
                {
                    await _semaphore.WaitAsync(stoppingToken);

                    tasks.Add(ProcessMessageAsync(message, stoppingToken));
                }

                await Task.WhenAll(tasks);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task ProcessMessageAsync(Message message, CancellationToken stoppingToken)
        {
            try
            {
                await messageService.UpdateMessageStatusAsync(message, MessageStatus.InProgress, stoppingToken);
                await strategyResolver.ResolveStrategyAsync(message, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error processing message {message.Id}");
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
