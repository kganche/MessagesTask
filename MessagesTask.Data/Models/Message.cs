using MessagesTask.Data.Models.Enums;

namespace MessagesTask.Data.Models;

public class Message
{
    public Guid Id { get; set; }

    public MessageType Type { get; set; }

    public MessageStatus Status { get; set; }

    public string MessageText { get; set; }
}
