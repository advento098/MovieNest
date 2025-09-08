using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MovieNestBackend;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]


    public class MoviesController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public MoviesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetPopularMovies([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Add something on the query");

            const string apiKey = "110ecaa3";
            string url = $"http://www.omdbapi.com/?s={Uri.EscapeDataString(query)}&apikey={apiKey}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "API request failed");

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var json2 = JsonSerializer.Deserialize<ResponseTemplate>(content, options);

            Console.WriteLine($"json2 is: {json2}");
            return Ok(json);
        }
    }
}
