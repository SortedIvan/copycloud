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
            string tokenBase = $"{inviteeEmail};{sender};{projectInvitedToId}";
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
            return Tuple.Create(inviteeEmail, projectInvitedToId, sender, secret);
            
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
