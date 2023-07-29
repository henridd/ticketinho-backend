using Google.Cloud.Firestore;
using Ticketinho.Repository.Converters;

namespace Ticketinho.Repository.Models
{
    [FirestoreData]
    public abstract class Model
    {
        [FirestoreProperty(ConverterType = typeof(GuidConverter))]
        public Guid Id { get; set; }

        public Model()
        {
            Id = Guid.NewGuid();
        }
    }
}
