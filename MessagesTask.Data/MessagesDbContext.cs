using MessagesTask.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MessagesTask.Data;

public class MessagesDbContext(DbContextOptions<MessagesDbContext> options) : DbContext(options)
{
    public DbSet<Message> Messages { get; set; }

    public DbSet<ProcessedMessage> ProcessedMessages { get; set; }
}
