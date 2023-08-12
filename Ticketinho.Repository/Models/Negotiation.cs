using Google.Cloud.Firestore;

namespace Ticketinho.Repository.Models
{
    [FirestoreData]
    public class Negotiation : Model
    {
        [FirestoreProperty]
        public DateTime CreatedAt { get; set; }

        [FirestoreProperty]
        public string TicketId { get; set; }

        [FirestoreProperty]
        public string SellerId { get; set; }

        [FirestoreProperty]
        public string BuyerId { get; set; }

        [FirestoreProperty]
        public bool HandshakePerformed { get; set; }

        [FirestoreProperty]
        public bool IsActive { get; set; }

        // Used when deserializing from the database
        public Negotiation() { }

        public Negotiation(string ticketId, string sellerId, string buyerId)
        {
            TicketId = ticketId ?? throw new ArgumentNullException(nameof(ticketId));
            SellerId = sellerId ?? throw new ArgumentNullException(nameof(sellerId));
            BuyerId = buyerId ?? throw new ArgumentNullException(nameof(buyerId));

            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public void PerformHandshake()
        {
            HandshakePerformed = true;
            IsActive = false;
        }

    }
}
