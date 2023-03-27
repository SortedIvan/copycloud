using contentservice.Dto;
using contentservice.Models;

namespace contentservice.Services
{
    public interface IWebsiteCopyService
    {
        Task<List<CtoCopyDto>> GenerateCallToActionCopy(int amount, string actionGoal, string actionContext, string actionType, string tone, int maxCharacters, string[]? samplePhrases);
        Task<string> GenerateCallToActionCopyTest(int amount, string actionGoal, string actionContext, string actionType, string tone, int maxCharacters, string[]? samplePhrases);
        Task<bool> SaveCopyToUser(CtoCopyDto copy, string userId);
        Task<CtoModel> GetCtoCopyByUserId(string userId);
    }
}
