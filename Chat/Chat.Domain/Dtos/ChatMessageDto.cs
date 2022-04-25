namespace Chat.Domain.Dtos
{
    public class ChatMessageDto
    {
        public Guid MessageId { get; set; }
        public string SenderUserId { get; set; }
        public string Message { get; set; }
        public DateTime SentDate { get; set; }
        public string Username { get; set; }
    }
}
