using Microsoft.AspNetCore.Mvc;

namespace AIAnalyticsReport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OAuthController : ControllerBase
    {
        private readonly OAuthManager _auth;

        public OAuthController(OAuthManager _auth)
        {
            this._auth = _auth;
        }

        [HttpPost]
        public async Task<object> Login([FromBody] BaseCredential credential)
        {
            try
            {
                var response = await _auth.Authorize(credential);

                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
