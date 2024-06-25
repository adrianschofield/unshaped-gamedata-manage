using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using unshaped_gamedata_manage.Models;

namespace unshaped_gamedata_manage.Controllers;

public class GameController : Controller
{
    private readonly ILogger<GameController> _logger;

    private readonly IConfiguration _configuration;
    
    public GameController(ILogger<GameController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        List<GameData> games = new List<GameData>();

        using (var client = new HttpClient()) {
            client.BaseAddress = new Uri("https://app-gamedata-westeurope-dev-001.azurewebsites.net/api/");
            client.DefaultRequestHeaders.Add("x-api-key", _configuration["x-api-key"]);

            var responseTask = client.GetAsync("gamedata");
            responseTask.Wait();

            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) {
                var readTask  = result.Content.ReadFromJsonAsync<List<GameData>>();
                readTask.Wait();

                // TODO check this is non null
                if (readTask.Result != null) {
                    games = readTask.Result;
                } else {
                    ModelState.AddModelError(string.Empty, "Result is null");
                }
            } else {
                // games = Enumerable.Empty<GameData>();

                ModelState.AddModelError(string.Empty, "Server error");

            }           
        }
        return View(games);
    }

    public IActionResult Dashboard()
    {
        Dashboard data = new Dashboard();

        using (var client = new HttpClient()) {
            client.BaseAddress = new Uri("https://app-gamedata-westeurope-dev-001.azurewebsites.net/api/");
            client.DefaultRequestHeaders.Add("x-api-key", _configuration["x-api-key"]);

            var responseTask = client.GetAsync("gamedata/dashboard");
            responseTask.Wait();

            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) {
                var readTask  = result.Content.ReadFromJsonAsync<Dashboard>();
                readTask.Wait();

                // TODO check this is non null

                data = readTask.Result;
            } else {
                // data = Enumerable.Empty<GameData>();

                ModelState.AddModelError(string.Empty, "Server error");

            }           
        }
        return View(data);
    }
}
