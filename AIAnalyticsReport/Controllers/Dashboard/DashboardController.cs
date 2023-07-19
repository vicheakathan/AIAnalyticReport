using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AIAnalyticsReport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardManager _dashboard;

        public DashboardController(DashboardManager _dashboard)
        {
            this._dashboard = _dashboard;
        }

        [HttpGet("PerformanceByDay")]
        public async Task<object> PerformanceByDay(string? tenant)
        {
            try
            {
                var response = _dashboard.PerformanceByDay(tenant);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
