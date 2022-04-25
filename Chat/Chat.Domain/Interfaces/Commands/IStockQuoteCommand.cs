using Microsoft.AspNetCore.SignalR;

namespace Chat.Domain.Interfaces.Commands
{
    public interface IStockQuoteCommand
    {
        public string SenderUserId { get; set; }
        public string StockCode { get; set; }
        public HubCallerContext HubCallerContext { get; set; }
        public IHubCallerClients Clients { get; set; }
    }
}