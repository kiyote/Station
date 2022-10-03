using Station.Common;
using Microsoft.AspNetCore.Mvc;

namespace BlazorPhaser.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public static readonly string[] Summaries = new[]
        {
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

#pragma warning disable IDE0052
		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(
			ILogger<WeatherForecastController> logger
		) {
            _logger = logger;
        }
#pragma warning restore IDE0052

#pragma warning disable CA5394
		[HttpGet]
        public IEnumerable<WeatherForecast> Get() {
			return Enumerable.Range( 1, 5 ).Select( index => new WeatherForecast {
				Date = DateTime.Now.AddDays( index ),
				TemperatureC = Random.Shared.Next( -20, 55 ),
				Summary = Summaries[Random.Shared.Next( Summaries.Length )]
			} )
			.ToArray();
		}
#pragma warning restore CA5394
	}
}
