using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace projectservice.Utility
{
    public static class InvitationTokenUtil
    {
        //{inviteeemail}:{projectinvitedto}:{invitor}:{secret}
        public static Tuple<string, string> CreateInvitationToken(string inviteeEmail, string projectInvitedToId, string sender)
        {
            string tokenBase = $"{inviteeEmail}:{sender}:{projectInvitedToId}";
            string secret = Guid.NewGuid().ToString();
            byte[] secretHash = sha256_hash(secret);
            tokenBase += $":{secret}";
            return Tuple.Create(tokenBase, secret);
        }

        public static Tuple<string, string, string, string> ParseInviteToken(string token)
        {
            string[] words = token.Split(':');
            string inviteeEmail = words[0];
            string sender = words[1];
            string projectInvitedToId = words[2];
            string secret = words[3];
            return Tuple.Create(inviteeEmail, projectInvitedToId, sender, secret);
            
        }

        public static byte[] sha256_hash(string value)
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));
                return result;
            }

        }
    }
}
