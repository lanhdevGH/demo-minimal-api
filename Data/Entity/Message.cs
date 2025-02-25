namespace WebSocketChatApp.Data.Entity
{
    public class Message
    {
        public int Id { get; set; }
        public Guid SenderId { get; set; } // ID của người gửi
        public Guid ReceiverId { get; set; } // ID của người nhận
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
