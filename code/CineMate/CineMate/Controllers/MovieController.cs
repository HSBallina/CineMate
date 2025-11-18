using Azure;
using Azure.AI.OpenAI;
using CineMate.Configuration;
using CineMate.Models;
using CineMate.Tmdb;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;
using System.Text.Json;
using TMDbLib.Client;

namespace CineMate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController(ITmdb tmdb, ApiSettings settings) : ControllerBase
{
    private readonly AzureOpenAIClient _openAiClient = new(
        new Uri(settings.OpenAiEndpoint),
        new AzureKeyCredential(settings.OpenAiKey));

    [HttpPost("recommend")]
    public async Task<IActionResult> RecommendMovie([FromBody] UserPreferences prefs)
    {
        var userChatMessage =
            $"Suggest up to six movies based on these preferences; Genre: {prefs.Genre}, Mood: {prefs.Mood}, Actor: {prefs.Actor}. " +
            "Return the result strictly as JSON in this format: " +
            "[ { \"title\": \"Movie Title\", \"year\": 1999 }, " +
            "{ \"title\": \"Another Movie\", \"year\": 2010 } ] " +
            "Do not include any extra text or explanation.";

        var chatClient = _openAiClient.GetChatClient("gpt-5.1-chat");
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("You are CineMate, an AI movie recommendation assistant."),
            new UserChatMessage(userChatMessage)
        };

        var response = await chatClient.CompleteChatAsync(messages, new ChatCompletionOptions()
        {
            Temperature = (float)1,
            FrequencyPenalty = (float)0,
            PresencePenalty = (float)0,
        });

        var gptMovies = JsonSerializer
            .Deserialize<List<GptMovie>>(response.Value.Content[0].Text.Trim());

        var tmdbMovie = await tmdb.GetMovie(gptMovies![0]);
        //var movieData = System.Text.Json.JsonSerializer.Deserialize<TmdbSearchResult>(tmdbResponse);

        //var result = new
        //{
        //    SuggestedMovie = gptSuggestion,
        //    Poster = $"https://image.tmdb.org/t/p/w500{movieData?.Results?.FirstOrDefault()?.PosterPath}",
        //    Rating = movieData?.Results?.FirstOrDefault()?.VoteAverage,
        //    Tagline = $"Perfect for a {prefs.Mood} night!"
        //};

        return Ok(tmdbMovie);

    }
}
