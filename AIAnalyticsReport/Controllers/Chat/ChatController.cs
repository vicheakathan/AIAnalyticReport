using AnalyticsReport.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AIAnalyticsReport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly ChatManager _chatGPT;

        private readonly IWebHostEnvironment _env;

        public ChatController(ChatManager _chatGPT, IWebHostEnvironment _env)
        {
            this._chatGPT = _chatGPT;
            this._env = _env;
        }

        [HttpPost("insight")]
        public async Task<object> Insight([FromBody] Chat prompt)
        {
            try
            {
                var response = await _chatGPT.GetInsights(prompt);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("fine-tuning")]
        public async Task<object> FineTuning()
        {
            try
            {
                var response = await _chatGPT.GetFineTuning();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
