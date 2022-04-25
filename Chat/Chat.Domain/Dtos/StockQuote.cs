namespace Chat.Domain.Dtos
{
    public class StockQuote
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public int Volume { get; set; }
    }
}