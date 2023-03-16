using Microsoft.AspNetCore.Mvc;

namespace contentservice.Controllers
{
    [ApiController]
    public class GatewayTestController :ControllerBase
    {
        [HttpGet("/api/testgatewaycontent")]
        public async Task<string> TestGateway()
        {
            return "Contentservice gateway works";
        }

    }

}