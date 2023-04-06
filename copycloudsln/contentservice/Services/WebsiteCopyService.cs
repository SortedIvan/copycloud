using contentservice.Data;
using contentservice.Dto;
using contentservice.Factories;
using contentservice.Models;
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
        private IWebsiteCopyFactory copyFactory;
        private IContentDbConfig contentDb;

        public WebsiteCopyService(IConfiguration _config, IOpenAIManager aiManager, IWebsiteCopyFactory _factory, IContentDbConfig _contentDb) 
        {
            config = _config;
            copyFactory = _factory;
            openaiManager = aiManager;
            contentDb = _contentDb;
        }

        public async Task<string> GenerateCallToActionCopyTest(int amount, string actionGoal, string actionContext, string actionType, string tone, int maxCharacters, string[]? samplePhrases)
        {
            Conversation chatgptconvesation = openaiManager.CreateChatGptConversation();

            // Prepend the initial instructions
            chatgptconvesation.AppendSystemMessage($"Generate {amount} call to action copywriting statements based on user input.");
            chatgptconvesation.AppendSystemMessage("Return these statements as a JSON Object with the structure {\"copies\": [{\"copy\": \"copy_content\"}]}.");

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

            return output;
        }


        public async Task<List<CtoCopyDto>> GenerateCallToActionCopy(int amount, string actionGoal, string actionContext, string actionType, string tone, int maxCharacters, string[]? samplePhrases)
        {
            Conversation chatgptconvesation = openaiManager.CreateChatGptConversation();

            // Prepend the initial instructions
            chatgptconvesation.AppendSystemMessage($"Generate {amount} call to action copywriting statements based on user input.");
            chatgptconvesation.AppendSystemMessage("Return these statements as a JSON Object with the structure {\"copies\": [{\"copy\": \"copy_content\"}]}.");

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
            List<CtoCopyDto> copyDtoList = new List<CtoCopyDto>();
            for (int i = 0; i < copies.Count; i++)
            {
                copyDtoList.Add(
                copyFactory.CreateCtoCopyDto(
                    copies.ElementAt(i).copy,
                    tone,
                    actionType,
                    actionContext
                    ));
            }
            return copyDtoList;
        }

        public async Task<bool> SaveCopyToUser(CtoCopyDto copy, string userId)
        {
            await contentDb.SaveCtoCopy(copy, userId);
            return true;
        }
        
        public async Task<CtoModel> GetCtoCopyByUserId(string userId)
        {
            CtoModel model = await contentDb.GetCtoCopyByUserId(userId);
            return model;
        }

        public async Task<List<CtoModel>> GetAllSavedCopies(string userId)
        {
            List<CtoModel> userCopies = await contentDb.GetAllUserSavedCtoCopies(userId);
            return userCopies;
        }
    }
}
