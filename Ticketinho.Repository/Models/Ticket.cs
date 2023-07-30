using Google.Cloud.Firestore;
using Ticketinho.Repository.Models.Enums;

namespace Ticketinho.Repository.Models
{
    [FirestoreData]

    public class Ticket : Model
    {
        [FirestoreProperty]
        public string OwnerId { get; set; }

        [FirestoreProperty]
        public TicketZone Zone { get; set; }

        [FirestoreProperty]
        public TicketType Type { get; set; }

        [FirestoreProperty]
        public double Price { get; set; }

        [FirestoreProperty]
        public bool IsActive { get; set; }

        public Ticket() { }

        public Ticket(string ownerId, TicketZone zone, TicketType type, double price)
        {
            OwnerId = ownerId;
            Zone = zone;
            Type = type;
            Price = price;
            IsActive = true;
        }
    }
}
