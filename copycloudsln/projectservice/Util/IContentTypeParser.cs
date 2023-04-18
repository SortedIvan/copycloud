namespace projectservice.Util
{
    public interface IContentTypeParser
    {
        Tuple<bool, string, string> TryParseObject(object content);
    }
}
