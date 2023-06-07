using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Firebase.Auth;
using Newtonsoft.Json;
using userservice.Database;
using userservice.Models;

namespace userservice.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDbConfig userDb;
        private readonly IConfiguration config;
        private FirebaseAuthProvider firebaseProvider;
        private readonly EventHubProducerClient producer;

        public UserService(IUserDbConfig _userDb, IConfiguration _config)
        {
            this.userDb = _userDb;
            this.config = _config;
            this.producer = new EventHubProducerClient(config.GetSection("EventHubConfig:ConnectionString").Value, config.GetSection("EventHubConfig:Hub").Value);
            firebaseProvider = new FirebaseAuthProvider(new FirebaseConfig(_config.GetSection("FirebaseSettings:firebase_api_key").Value));
        }

        public async Task<Tuple<bool, string>> DeleteUser(string userEmail, string token)
        {
            bool result = await userDb.DeleteUser(userEmail);
            
            if (result)
            {
                try
                {
                    await firebaseProvider.DeleteUserAsync(token);

                    DeleteUserData data = new DeleteUserData
                    {
                        UserEmail = userEmail
                    };

                    ProjectEvent projectEvent = new ProjectEvent
                    {
                        EventType = "deleteuser",
                        DeleteUserData = data
                    };
                    string messageBody = JsonConvert.SerializeObject(projectEvent);

                    List<EventData> emailEvent = new List<EventData>
                    {
                        new EventData(messageBody)
                    };
                    await producer.SendAsync(emailEvent); // Email is sent as event to hub
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return Tuple.Create(false, "Something went wrong, user couldn't be deleted");
                }
                return Tuple.Create(true, $"User with email: {userEmail} has beel deleted.");

            }
            return Tuple.Create(false, "Something went wrong, user couldn't be deleted");
        }
    }
}
