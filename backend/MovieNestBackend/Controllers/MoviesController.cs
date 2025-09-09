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
        public async Task<IActionResult> SearchMovies([FromQuery] string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return BadRequest("Add something on the s");

            string? apiKey = Environment.GetEnvironmentVariable("API_KEY");

            if (apiKey == null || apiKey == "")
                return Content("No api key found");

            string url = $"http://www.omdbapi.com/?s={Uri.EscapeDataString(s)}&apikey={apiKey}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "API request failed");

            try
            {
                var content = await response.Content.ReadAsStringAsync();
                using var stringJson = JsonDocument.Parse(content);
                var movies = stringJson.RootElement.GetProperty("Search").GetRawText();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<List<Movies>>(movies, options);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"There's no such thing as string, error: {e}");
            }
        }
    }
}
