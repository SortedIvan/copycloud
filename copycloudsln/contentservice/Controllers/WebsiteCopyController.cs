using contentservice.Dto;
using contentservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace contentservice.Controllers
{
    [ApiController]
    public class WebsiteCopyController : ControllerBase
    {
        private IWebsiteCopyService websiteCopyService;
        public WebsiteCopyController(IWebsiteCopyService _websiteCopyService)
        {
            this.websiteCopyService = _websiteCopyService;
        }

        [HttpPost("/api/getctocopies")]
        public async Task<List<Copy>> GetCallToActionCopies(int amount, string actionGoal, string actionContext, string actionType, string tone, int maxCharacters, string[]? samplePhrases)
        {
            List<Copy> copy = await websiteCopyService.GenerateCallToActionCopy(amount, actionGoal, actionContext, actionType, tone, maxCharacters, samplePhrases);
            return copy;
        }

    }
}
