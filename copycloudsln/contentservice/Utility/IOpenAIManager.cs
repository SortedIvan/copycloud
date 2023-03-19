using OpenAI_API;
using OpenAI_API.Chat;

namespace contentservice.Utility
{
    public interface IOpenAIManager
    {
        OpenAIAPI GetAPI();
        Conversation CreateChatGptConversation();
    }
}
