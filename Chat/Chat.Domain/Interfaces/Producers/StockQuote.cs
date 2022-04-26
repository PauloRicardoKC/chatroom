namespace Chat.Domain.Interfaces
{
    public interface StockQuote
    {
        public string SenderUserId { get; set; }
        public string StockCode { get; set; }
    }
}