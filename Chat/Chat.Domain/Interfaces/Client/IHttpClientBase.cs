namespace Chat.Domain.Interfaces.Client
{
    public interface IHttpClientBase
    {
        Task<TResponse> GetAsync<TResponse>(IDictionary<string, string> headers, string uri);
    }
}
