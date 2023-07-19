using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIAnalyticsReport.Controllers.Google
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GoogleController : ControllerBase
    {
        private readonly GoogleManager _google;

        public GoogleController(GoogleManager google)
        {
            this._google = google;
        }

        [HttpGet]
        public async Task<object> Get([FromBody] GoogleVertexAI entity)
        {
            try
            {
                var response = await _google.Chat(entity);

                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
