using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIAnalyticsReport.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class DinexMachineLearningController : ControllerBase
	{
		private readonly DinexMachineLearningManager _machineLearning;

		public DinexMachineLearningController(DinexMachineLearningManager _machineLearning)
		{
			this._machineLearning = _machineLearning;
		}

		[HttpGet]
		public async Task<object> Get()
		{
			try
			{
				var response = await _machineLearning.InvokeRequestResponseService();

				return Ok(response);
			}
			catch(Exception ex)
			{
				return BadRequest(ex);
			}
		}
	}
}

