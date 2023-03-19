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

        [HttpPost("/api/ctocopy")]
        public async Task<string> GetCallToActionCopies(int amount, string actionGoal, string actionContext, string actionType, string tone, int maxCharacters, string[]? samplePhrases)
        {
            string copy = await websiteCopyService.GenerateCallToActionCopy(amount, actionGoal, actionContext, actionType, tone, maxCharacters, samplePhrases);
            return copy;
        }


    }
}
