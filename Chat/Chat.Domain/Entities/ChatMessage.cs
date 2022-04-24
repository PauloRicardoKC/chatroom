namespace Chat.Domain.Entities
{
    public class ChatMessage
    {
        public Guid MessageId { get; set; }
        public string SenderUserId { get; set; }
        public string Message { get; set; }        
        public DateTime SentDate { get; set; }        
    }
}