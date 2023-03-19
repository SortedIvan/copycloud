namespace contentservice.Services
{
    public interface IWebsiteCopyService
    {
        Task<string> GenerateCallToActionCopy(int amount, string actionGoal, string actionContext, string actionType, string tone, int maxCharacters, string[]? samplePhrases);
    }
}
