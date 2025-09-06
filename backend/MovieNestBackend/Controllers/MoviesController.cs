using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetPopularMovies()
        {
            const string apiKey = "110ecaa3";
            const string url = $"http://www.omdbapi.com/?s=spiderman&apikey={apiKey}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "API request failed");

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);

            return Ok(json);
        }
    }
}
