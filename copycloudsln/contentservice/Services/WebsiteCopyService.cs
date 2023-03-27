using contentservice.Dto;
using contentservice.Utility;
using OpenAI_API.Chat;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.Intrinsics.Arm;

namespace contentservice.Services
{
    public class WebsiteCopyService : IWebsiteCopyService
    {
        private IConfiguration config;
        private IOpenAIManager openaiManager;
        public WebsiteCopyService(IConfiguration _config, IOpenAIManager aiManager) 
        {
            config = _config;
            openaiManager = aiManager;
        }


        public async Task<List<Copy>> GenerateCallToActionCopy(int amount, string actionGoal, string actionContext, string actionType, string tone, int maxCharacters, string[]? samplePhrases)
        {
            Conversation chatgptconvesation = openaiManager.CreateChatGptConversation();

            // Prepend the initial instructions
            chatgptconvesation.AppendSystemMessage($"Generate {amount} call to action copywriting statements based on user input.");
            chatgptconvesation.AppendSystemMessage("Return these statements as a JSON Object with the structure {\"copies\": [{\"copy\": \"copy_content\"}]}. Do not return any non-json text or numbering");

            chatgptconvesation.AppendUserInput("The action goal is: " + actionGoal);
            chatgptconvesation.AppendUserInput("The action context is: " + actionContext);
            chatgptconvesation.AppendUserInput("The action type is: " + actionType);
            chatgptconvesation.AppendUserInput("Set the tone of the copy as: " + tone);

            string prependPhrases = "";
            if (samplePhrases != null)
            {
                for (int i = 0; i < samplePhrases.Length; i++)
                {
                    prependPhrases += samplePhrases[i];
                }
            }

            chatgptconvesation.AppendUserInput("Here are some sample phrases: " + prependPhrases);

            string output = await chatgptconvesation.GetResponseFromChatbot();

            List<Copy> copies = ChatgptRegex.ParseCopy(output); // Parse the copy objects into seperate json objects

            return copies;
        }


    }
}
