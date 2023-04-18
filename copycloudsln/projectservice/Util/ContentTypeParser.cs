using ContentLibrary;

namespace projectservice.Util
{
    public class ContentTypeParser : IContentTypeParser
    {
        public Tuple<bool, string, string> TryParseObject(object content)
        {
            if (content.GetType() == typeof(CtoModel))
            {
                CtoModel parsedModel = (CtoModel)content;
                
                return Tuple.Create(true, "cto", parsedModel.Id);
            }
            return Tuple.Create(false, "none", "none");

        }
    }
}