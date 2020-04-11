using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VGT.Common;

namespace VGT.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly CosmosClient _client;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, CosmosClient client, IConfiguration configuration)
        {
            _client = client;
            _logger = logger;
            _configuration = configuration;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
        {
            List<WeatherForecast> returnVal = new List<WeatherForecast>();
            Container weatherForecastContainer = await GetWeatherForecastContainerAsync();
            QueryDefinition queryDef = new QueryDefinition(string.Format("select * from WeatherForecast", "WeatherForecast"));
            FeedIterator<WeatherForecast> allItemsInResult = weatherForecastContainer.GetItemQueryIterator<WeatherForecast>(
                queryDefinition: queryDef,
                continuationToken: null);

            while (allItemsInResult.HasMoreResults)
                foreach (var item in await allItemsInResult.ReadNextAsync())
                    returnVal.Add(item);

            return Ok(returnVal);
        }
        [HttpPost]
        public async Task<ActionResult> Post(WeatherForecast _weatherForecast)
        {
            await (await GetWeatherForecastContainerAsync()).CreateItemAsync<WeatherForecast>(_weatherForecast);
            return Ok();
        }
        private async Task<Container> GetWeatherForecastContainerAsync()
        {
            return await (await _client.CreateDatabaseIfNotExistsAsync(_configuration.GetSection("CosmosDb")["DatabaseName"])).Database.CreateContainerIfNotExistsAsync("WeatherForecast", "/Id");
        }

    }
}
