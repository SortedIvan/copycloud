namespace projectservice.Utility
{
    public interface IPusherHelper
    {
        Task<string> AuthenticatePusher(string channelName, string socketId, string userId, string email);
    }
}
