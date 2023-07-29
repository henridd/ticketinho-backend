using Google.Cloud.Firestore;

namespace Ticketinho.Repository.Converters
{
    internal class GuidConverter : IFirestoreConverter<Guid>
    {
        public Guid FromFirestore(object value)
        {
            return Guid.Parse(value.ToString());
        }

        public object ToFirestore(Guid value)
        {
            return value.ToString();
        }
    }
}
