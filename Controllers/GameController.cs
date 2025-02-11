using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FreeToGameApi.Models;

// Define HttpClient instance to make requests to FreeToGame API
[Route("api/games")]
[ApiController]
public class GameController : ControllerBase
{
    private static readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://www.freetogame.com/api/")
    };

    // GET live games list
    [HttpGet("all")]
    public async Task<IActionResult> GetAllGames()
    {
        var response = await _httpClient.GetAsync("games");
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var games = JsonSerializer.Deserialize<List<Game>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return Ok(games);
    }

    // GET details from specific game by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGameById(int id)
    {
        var response = await _httpClient.GetAsync($"game?id={id}");
        if (!response.IsSuccessStatusCode)
            return NotFound($"Game with {id} couldnt be found");

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var game = JsonSerializer.Deserialize<Game>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return Ok(game);
    }
    
    // Endpoint to filter games based on category, platform and sorting criteria
    [HttpGet]
    public async Task<IActionResult> GetGamesByFilter([FromQuery] string? category = null, [FromQuery] string? platform = null, [FromQuery] string? sortBy = null)
    {
        var queryParams = new List<string>();

        // Add query parameters if not provided
        if (!string.IsNullOrEmpty(category)) queryParams.Add($"category={category}");
        if (!string.IsNullOrEmpty(platform)) queryParams.Add($"platform={platform}");
        if (!string.IsNullOrEmpty(sortBy)) queryParams.Add($"sort-by={sortBy}");

        // Construct request URI with queryparams
        string requestUri = "games";
        if (queryParams.Count > 0)
        {
            requestUri += "?" + string.Join("&", queryParams);
        }

        var response = await _httpClient.GetAsync(requestUri);
        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "Error fetching data from FreeToGame API");
        //Deserialize JSON response into list of Game objects
        //'PropertyNameCaseInsensitive' to ensure JSON property names 
        //are matched to C# properties regardless of letter casing.
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var games = JsonSerializer.Deserialize<List<Game>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return Ok(games);
    }
}
