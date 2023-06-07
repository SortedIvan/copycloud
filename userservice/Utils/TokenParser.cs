namespace userservice.Utils
{
    public static class TokenParser
    {
        public static string ParseToken(string input)
        {
            const string tokenKey = "token=";

            int startIndex = input.IndexOf(tokenKey);
            if (startIndex == -1)
                throw new ArgumentException("Invalid input string: token key not found");

            startIndex += tokenKey.Length;
            int endIndex = input.IndexOf(';', startIndex);
            if (endIndex == -1)
                endIndex = input.Length;

            return input.Substring(startIndex, endIndex - startIndex);
        }
    }
}
