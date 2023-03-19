using OpenAI_API;
using OpenAI_API.Chat;

namespace contentservice.Utility
{
    public class OpenAIManager : IOpenAIManager
    {
        private IConfiguration config;
        private OpenAIAPI openaiapi;
        public OpenAIManager(IConfiguration _config) 
        {
            this.config = _config;
            this.openaiapi = new OpenAIAPI(config.GetSection("OpenAISettings:Key").Value);
        }

        public OpenAIAPI GetAPI()
        {
            return openaiapi;
        }

        public Conversation CreateChatGptConversation()
        {
            return this.openaiapi.Chat.CreateConversation();
        }
    }
}
