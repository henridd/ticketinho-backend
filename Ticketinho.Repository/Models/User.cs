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
        public string Password { get; set; }

        [FirestoreProperty]
        public string PhoneNumber { get; set; }

        public User(string name, string email, string password, string phoneNumber)
            : base()
        {
            Name = name;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
        }

        public User()
        {

        }
    }
}

