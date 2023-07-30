using Google.Cloud.Firestore;

namespace Ticketinho.Repository.Models
{
    [FirestoreData]
	public class User : Model
	{
        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Email { get; set; }

        [FirestoreProperty]

        public string HashedPassword { get; set; }

        [FirestoreProperty]
        public byte[] Salt { get; set; }

        [FirestoreProperty]
        public string PhoneNumber { get; set; }

        public User()
        {

        }
    }
}

