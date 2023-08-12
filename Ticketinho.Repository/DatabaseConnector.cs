using Google.Cloud.Firestore;

namespace Ticketinho.Repository
{
    public static class DatabaseConnector
    {
        public static FirestoreDb ConnectToDatabase()
        {
            using StreamReader r = new StreamReader("secrets.json");
            string json = r.ReadToEnd();

            return new FirestoreDbBuilder
            {
                ProjectId = "ticketinho",
                JsonCredentials = json
            }.Build();
        }
    }
}
