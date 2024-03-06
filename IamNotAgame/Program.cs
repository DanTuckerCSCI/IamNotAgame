using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

namespace IamNotAgame
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            // Read game description, instructions, and JavaScript code from files
            string gameDescription = File.ReadAllText("gameDescription.txt");
            string gameInstructions = File.ReadAllText("gameInstructions.txt");
            string gameJavaScript = File.ReadAllText("game.js");

            // Assume the image file and leaderboard are stored at these paths
            string imagePath = "gameThumbnail.jpg";
            string leaderboardPath = "leaderboard.json";

            // Reading the leaderboard
            var leaderboard = ReadLeaderboard(leaderboardPath);

            // Preparing data to send to the API
            var gameData = new
            {
                Description = gameDescription,
                Instructions = gameInstructions,
                JavaScriptCode = gameJavaScript,
                Leaderboard = leaderboard
            };
            string json = JsonSerializer.Serialize(gameData);

            // Sending data to the API
            await SendDataToApi(json);
        }

        static List<ScoreEntry> ReadLeaderboard(string path)
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<List<ScoreEntry>>(json) ?? new List<ScoreEntry>();
            }
            return new List<ScoreEntry>();
        }

        static async Task SendDataToApi(string jsonData)
        {
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync("YOUR_API_ENDPOINT", content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response from API: {responseBody}");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"\nException Caught!");
                Console.WriteLine($"Message :{e.Message}");
            }
        }

        // Define your ScoreEntry class here
        class ScoreEntry
        {
            public string PlayerName { get; set; }
            public int Score { get; set; }
        }
    }
}


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
