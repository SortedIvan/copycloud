using contentservice.Dto;

namespace contentservice.Services
{
    public interface IWebsiteCopyService
    {
        Task<List<Copy>> GenerateCallToActionCopy(int amount, string actionGoal, string actionContext, string actionType, string tone, int maxCharacters, string[]? samplePhrases);
        Task<string> GenerateAi(int amount, string actionGoal, string actionContext, string actionType, string tone, int maxCharacters, string[]? samplePhrases);
    }
}
