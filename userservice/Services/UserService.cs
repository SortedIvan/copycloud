using Azure.Messaging.EventHubs.Producer;
using userservice.Database;

namespace userservice.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDbConfig userDb;
        private readonly IConfiguration config;
        private readonly EventHubProducerClient producer;

        public UserService(IUserDbConfig _userDb, IConfiguration _config)
        {
            this.userDb = _userDb;
            this.config = _config;
            this.producer = new EventHubProducerClient(config.GetSection("EventHubConfig:ConnectionString").Value, config.GetSection("EventHubConfig:Hub").Value);
        }

        public async Task<Tuple<bool, string>> DeleteUser(string userEmail)
        {
            bool result = await userDb.DeleteUser(userEmail);

            if (result)
            {
                return Tuple.Create(true, $"User with email: {userEmail} has beel deleted.");
            }
            return Tuple.Create(false, "Something went wrong, user couldn't be deleted");
        }
    }
}
