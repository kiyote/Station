using Microsoft.AspNetCore.Mvc;

namespace BlazorPhaser.Server.Controllers {
	[ApiController]
	[Route( ".healthcheck" )]
	public class HealthcheckController : ControllerBase {

		[HttpGet]
		public IActionResult Get() {
			return Ok();
		}
	}
}
