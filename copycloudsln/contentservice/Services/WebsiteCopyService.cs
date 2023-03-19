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

        public async Task<string> GenerateCallToActionCopy(int amount, string actionGoal, string actionContext, string actionType, string tone, int maxCharacters, string[]? samplePhrases)
        {
            Conversation chatgptconvesation = openaiManager.CreateChatGptConversation();

            // Prepend the initial instructions

            chatgptconvesation.AppendSystemMessage(
                "You are going to generate call to action copywriting based on user input." +
                "The parameters provided will the the action's goal, the action context, the action type, action tone and optional sample phrases." +
                "Answer in a formatted way in a call to action copywrite style." +
                "The max characters for each copy is: " + maxCharacters);
            chatgptconvesation.AppendSystemMessage("The amount of different call to action copies you generate is: " + amount);
            chatgptconvesation.AppendSystemMessage("Return your result in a json format. It should be an array of phrases");
            chatgptconvesation.AppendSystemMessage("You will say nothing else but the call to action copywriting json array");

            chatgptconvesation.AppendSystemMessage("Return your result in a json array format.");


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

            List<string> copies = new List<string>();
            //string response = await chatgptconvesation.GetResponseFromChatbot();
            string temp_phrase = "";
            //while (true)
            //{
            //    try
            //    {
            //        response = 
            //        break; // success!
            //    }
            //    catch (Exception e)
            //    {
            //        Debug.WriteLine(e.Message);
            //    }
            //}


            //for (int i = 0; i < response.Length; i++)
            //{
            //    if (response[i] != ';')
            //    {
            //        temp_phrase += response[i];
            //    }
            //    else
            //    {
            //        copies.Append(temp_phrase);
            //        temp_phrase = "";
            //    }
            //}

            return await chatgptconvesation.GetResponseFromChatbot();
        }
    }
}



//const promptText = `Given the article below, create a JSON object which enumerates a set of 5 child objects.                       
//                        Each child object has a property named "q" and a property named "a".
//                        For each child object assign to the property named "q" a question which has its answer in the article 
//                        and to the property named "a" a short answer to this question.
//                        The resulting JSON object should be in this format: [{"q":"string","a":"string"}].\n\n
//                        The article:\n
//                        ${ textToUse}\n\n
//                        The JSON object:\n\n`;