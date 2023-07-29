using Google.Cloud.Firestore;

namespace Ticketinho.Repository.Models
{
    [FirestoreData]
    public abstract class Model
    {
        [FirestoreDocumentId]
        public string Id { get; set; }
    }
}
