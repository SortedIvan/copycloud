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

            chatgptconvesation.AppendSystemMessage("Generate call to action copywriting based on user input.");
            chatgptconvesation.AppendSystemMessage($"Create a JSON object which enumerates a set of {amount} child objects, each one of which is a different call to action copy");
            chatgptconvesation.AppendSystemMessage("Return nothing but the JSON object.");

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