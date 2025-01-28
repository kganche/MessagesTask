using MessagesTask.Data;
using MessagesTask.Data.Models.Enums;
using MessagesTask.Services.ApplicationServices;
using MessagesTask.Services.ApplicationServices.Contracts;
using MessagesTask.Services.BackgroundServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MessagesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddHostedService<MessageProcessorBackgroundService>();

builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSingleton<IProcessedMessageService, ProcessedMessageService>();

builder.Services.AddKeyedSingleton<IMessageProcessorService, FileMessageProcessorService>(MessageType.File);
builder.Services.AddKeyedSingleton<IMessageProcessorService, DatabaseProcessorService>(MessageType.Database);
builder.Services.AddKeyedSingleton<IMessageProcessorService, NotificationMessageProcessorService>(MessageType.Notification);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
