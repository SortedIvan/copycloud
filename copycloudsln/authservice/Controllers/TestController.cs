using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authservice.Controllers
{
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet("api/testcontroller")]
        public async Task<string> TestEndpoint()
        {
            return "This is secret data";
        }
    }
}
