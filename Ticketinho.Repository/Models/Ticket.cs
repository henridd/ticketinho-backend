using Google.Cloud.Firestore;
using Ticketinho.Repository.Models.Enums;

namespace Ticketinho.Repository.Models
{
    [FirestoreData]

    public class Ticket : Model
    {
        [FirestoreProperty]
        public TicketZone Zone { get; set; }

        [FirestoreProperty]
        public TicketType Type { get; set; }

        [FirestoreProperty]
        public double Price { get; set; }
    }
}
