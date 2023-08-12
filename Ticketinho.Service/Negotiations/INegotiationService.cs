namespace Ticketinho.Service.Negotiations
{
    public interface INegotiationService
    {
        Task<string> AddAsync(string ticketId, string buyerId, string sellerId);
        Task CancelNegotiationAsync(string id);
        Task PerformHandshakeAsync(string id);
    }
}
