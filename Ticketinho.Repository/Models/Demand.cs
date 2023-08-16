using Google.Cloud.Firestore;
using Ticketinho.Common.Enums;

namespace Ticketinho.Repository.Models
{
    [FirestoreData]
    public class Demand : Model
    {
        [FirestoreProperty]
        public string RequesterId { get; set; }

        [FirestoreProperty]
        public TicketZone Zone { get; set; }

        [FirestoreProperty]
        public TicketType Type { get; set; }

        [FirestoreProperty]
        public double Price { get; set; }

        [FirestoreProperty]
        public bool IsActive { get; set; }

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; }

        // Used when deserializing from the database
        public Demand() { }

        public Demand(string requesterId, TicketZone zone, TicketType type, double price)
        {
            RequesterId = requesterId;
            Zone = zone;
            Type = type;
            Price = price;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
