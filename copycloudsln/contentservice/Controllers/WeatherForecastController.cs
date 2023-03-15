using Microsoft.AspNetCore.Mvc;

namespace contentservice.Controllers
{
    [ApiController]
    public class GatewayTestController :ControllerBase
    {
        [HttpGet("/api/testapigateway")]
        public async Task<IActionResult> TestGateway()
        {
            return Ok("Gateway works!");
        }
    
    
    
    
    
    
    }

}