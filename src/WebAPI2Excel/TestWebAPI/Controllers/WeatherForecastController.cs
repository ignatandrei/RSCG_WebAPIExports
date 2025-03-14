using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace TestWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public async Task<IEnumerable<int>> Testint()
        {
            await Task.Delay(100);
            return new int[3] { 1970,4,16 };
        }
        [HttpGet]
        private async Task<IEnumerable<string>> TestMyString()
        {
            await Task.Delay(100);
            return new string[2] { "Andrei", "Ignat" };
        }
        [HttpGet]
        public async Task<Dictionary<string, object>[]> TestDictionary()
        {
            await Task.Delay(100);
            var ret= new Dictionary<string, object>[2];
            ret[0] = new Dictionary<string, object>();
            ret[0].Add  ("FirstName", "Andrei");
            ret[0].Add("ID", 1);
            ret[1] = new Dictionary<string, object>();
            ret[1].Add("FirstName", "Ignat");
            ret[1].Add("ID", 2);
            return ret; 
        }
        [HttpGet]
        public async Task<IEnumerable<string>> TestString()
        {
            await Task.Delay(100);
            return new string[2] { "Andrei", "Ignat" };
        }
        [HttpGet]
        public int Count()
        {
            return 10;
        }
        [HttpPost]
        public WeatherForecast[] TestNew()
        {
            return GetWeatherForecast().ToArray();
        }
        [HttpDelete]
        public Results<Ok<WeatherForecast[]>, NotFound<string>> TestResults()
        {
            var data = GetWeatherForecast().ToArray();
            if ((data?.Length ?? 0) == 0) return TypedResults.NotFound("cannot find");
            return TypedResults.Ok(data);
        }
        [HttpGet]
        public Person GetPerson(int id)
        {
            return new Person() { Id = id, Name = "test" + id };
        }

        [HttpPut]
        public ActionResult<WeatherForecast[]> TestActionResult()
        {
            return GetWeatherForecast().ToArray();
        }
        [HttpGet()]
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                NewItem = new WeatherForecast()
                {
                    TemperatureC = 100
                }
            })
            .ToArray();
        }
    }

}