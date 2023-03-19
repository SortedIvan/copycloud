using System.Text.RegularExpressions;

namespace contentservice.Utility
{
    public static class ChatgptRegex
    {
        public static string[] GetValuesWithDelimiter(string input)
        {
            string[] phrases = Regex.Split(input, "||");
            return phrases;
        }

    }
}
