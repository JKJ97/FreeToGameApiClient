using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FreeToGameApi.Models;

[Route("api/games")]
[ApiController]
public class GameController : ControllerBase
{
    private static readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://www.freetogame.com/api/")
    };

    [HttpGet("all")]
    public async Task<IActionResult> GetAllGames()
    {
        var response = await _httpClient.GetAsync("games");
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var games = JsonSerializer.Deserialize<List<Game>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return Ok(games);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGameById(int id)
    {
        var response = await _httpClient.GetAsync($"game?id={id}");
        if (!response.IsSuccessStatusCode)
            return NotFound($"Peli ID:llä {id} ei löytynyt.");

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var game = JsonSerializer.Deserialize<Game>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return Ok(game);
    }
}

