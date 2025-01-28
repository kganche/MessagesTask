using MessagesTask.Data.Models.Enums;

namespace MessagesTask.Data.Models;

public class ProcessedMessage
{
    public Guid Id { get; set; }

    public MessageType Type { get; set; }

    public string MessageText { get; set; }

    public DateTime ProcessedOn { get; set; }
}
