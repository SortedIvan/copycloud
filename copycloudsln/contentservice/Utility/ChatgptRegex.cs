using contentservice.Dto;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
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

        public static List<Copy> ParseCopy(string jsonString)
        {
            CopyData copyData = JsonConvert.DeserializeObject<CopyData>(jsonString);
            return copyData.Copies;
        }


    }
}
