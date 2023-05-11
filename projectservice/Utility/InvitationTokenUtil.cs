using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace projectservice.Utility
{
    public static class InvitationTokenUtil
    {
        //{inviteeemail}:{invitor}:{projectinvitedto}:{secret}
        public static Tuple<string, string> CreateInvitationToken(string inviteeEmail, string projectInvitedToId, string sender)
        {
            string tokenBase = $"{inviteeEmail};{sender};{projectInvitedToId}";
            string secret = Guid.NewGuid().ToString();
            tokenBase += $";{secret}";
            return Tuple.Create(tokenBase, secret);
        }

        //{sender}:{projectId}:{secret}
        public static Tuple<string, string> CreateInvitationForLink(string projectId, string sender)
        {
            string tokenBase = $"{sender};{projectId}";
            string secret = Guid.NewGuid().ToString();
            tokenBase += $";{secret}";
            return Tuple.Create(tokenBase, secret);
        }

        public static Tuple<string, string, string, string> ParseInviteToken(string token)
        {
            string[] words = token.Split(';');
            string inviteeEmail = words[0];
            string sender = words[1];
            string projectInvitedToId = words[2];
            string secret = words[3];
            return Tuple.Create(inviteeEmail, sender, projectInvitedToId, secret);
        }

        public static Tuple<string, string, string> ParseInviteLink(string token)
        {
            string[] words = token.Split(';');
            string sender = words[0];
            string projectId = words[1];
            string secret = words[2];
            return Tuple.Create(sender, projectId, secret);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string sha256_hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
