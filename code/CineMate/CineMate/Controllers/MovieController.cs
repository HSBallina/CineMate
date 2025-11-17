using Azure;
using Azure.AI.OpenAI;
using CineMate.Configuration;
using CineMate.Models;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;
using System.Net.Http;
using System.Text.Json;

namespace CineMate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController(IHttpClientFactory httpClientFactory, ApiSettings settings) : ControllerBase
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
    private readonly AzureOpenAIClient _openAiClient = new(
        new Uri(settings.OpenAiEndpoint),
        new AzureKeyCredential(settings.OpenAiKey));

    [HttpPost("recommend")]
    public async Task<IActionResult> RecommendMovie([FromBody] UserPreferences prefs)
    {
        var chatClient = _openAiClient.GetChatClient("gpt-5.1-chat");
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("You are CineMate, an AI movie recommendation assistant."),
            new UserChatMessage($"Suggest up to 6 movies for Genre: {prefs.Genre}, Mood: {prefs.Mood}, Actor: {prefs.Actor}. Include a fun tagline.")
        };


        var response = await chatClient.CompleteChatAsync(messages, new ChatCompletionOptions()
        {
            Temperature = (float)1,
            FrequencyPenalty = (float)0,
            PresencePenalty = (float)0,
        });
        var chatResponse = response.Value.Content.Last().Text;
        //var gptSuggestion = response.Value.Choices[0].Message.Content.Trim();

        //// 3. Call TMDb API for real data
        //var tmdbResponse = await _httpClient.GetStringAsync(
        //    $"https://api.themoviedb.org/3/search/movie?query={Uri.EscapeDataString(gptSuggestion)}&api_key=YOUR-TMDB-KEY");

        //var movieData = System.Text.Json.JsonSerializer.Deserialize<TmdbSearchResult>(tmdbResponse);

        //var result = new
        //{
        //    SuggestedMovie = gptSuggestion,
        //    Poster = $"https://image.tmdb.org/t/p/w500{movieData?.Results?.FirstOrDefault()?.PosterPath}",
        //    Rating = movieData?.Results?.FirstOrDefault()?.VoteAverage,
        //    Tagline = $"Perfect for a {prefs.Mood} night!"
        //};

        return Ok(response);

    }
}
