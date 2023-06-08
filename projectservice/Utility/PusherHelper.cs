using PusherServer;

namespace projectservice.Utility
{
    public class PusherHelper : IPusherHelper
    {
        private readonly IConfiguration config;
        string APP_ID;
        string APP_KEY;
        string APP_SECRET;
        private Pusher pusher;

        public PusherHelper(IConfiguration _config)
        {
            this.config = _config;
            //APP_ID = config.GetSection("PusherSettings:PUSHER_APP_ID").Value;
            //APP_KEY = config.GetSection("PusherSettings:PUSHER_APP_KEY").Value;
            //APP_SECRET = config.GetSection("PusherSettings:PUSHER_APP_SECRET").Value;
            //var options = new PusherOptions
            //{
            //    Cluster = config.GetSection("PusherSettings:PUSHER_APP_CLUSTER").Value,
            //    Encrypted = true,
            //};

            //pusher = new Pusher(APP_ID, APP_KEY, APP_SECRET, options);
        }

        public async Task<string> AuthenticatePusher(string channelName, string socketId, string userId, string email)
        {
            var channelData = new PresenceChannelData
            {
                user_id = userId,
                user_info = new
                {
                    userEmail = email
                }
            };
            var auth = pusher.Authenticate(channelName, socketId, channelData);
            string json = auth.ToJson();

            return json;
        }

        public Pusher GetPusher()
        {
            return pusher;
        }

    }
}
