using contentservice.Dto;
using contentservice.Models;
using contentservice.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<List<CtoCopyDto>> GetCallToActionCopies(int amount, string actionGoal, string actionContext, string actionType, string tone, int maxCharacters, string[]? samplePhrases)
        {
            List<CtoCopyDto> copy = await websiteCopyService.GenerateCallToActionCopy(amount, actionGoal, actionContext, actionType, tone, maxCharacters, samplePhrases);
            return copy;
        }

        [HttpPost("/api/getctoresponse")]
        public async Task<string> GetChatgptResponse(int amount, string actionGoal, string actionContext, string actionType, string tone, int maxCharacters, string[]? samplePhrases)
        {
            string copy = await websiteCopyService.GenerateCallToActionCopyTest(amount, actionGoal, actionContext, actionType, tone, maxCharacters, samplePhrases);
            return copy;
        }

        [HttpPost("/api/savewebsitecopy")]
        public async Task<IActionResult> SaveWebsiteCopy(CtoCopyDto copyDto)
        {
            try
            {
                var reqUserId = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "id").FirstOrDefault();

                if (reqUserId == null)
                {
                    return BadRequest("No such copy or user exists. Please log in or refresh the page.");
                }
                string userId = reqUserId.Value;

                await websiteCopyService.SaveCopyToUser(copyDto, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/api/getwebsitecopybyuserid")]
        public async Task<ActionResult<CtoModel>> GetCopyByUserId()
        {
            var reqUserId = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "id").FirstOrDefault();

            if (reqUserId == null)
            {
                return BadRequest("No such copy or user exists. Please log in or refresh the page.");
            }

            string userId = reqUserId.Value;
            return Ok(await websiteCopyService.GetCtoCopyByUserId(userId));
        }
    }
}
