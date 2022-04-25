namespace Chat.Domain.Interfaces.Commands
{
    public interface IStockQuoteCommand
    {
        public string SenderUserId { get; set; }
        public string StockCode { get; set; }
    }
}