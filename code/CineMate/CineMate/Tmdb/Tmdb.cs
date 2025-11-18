using CineMate.Configuration;
using CineMate.Models;
using TMDbLib.Client;

namespace CineMate.Tmdb;

public class Tmdb(ApiSettings settings) : ITmdb
{
    private readonly TMDbClient _tMDbClient = new(settings.TmdbApiKey);

    public async Task<TmdbMovie?> GetMovie(GptMovie gptMovie)
    {
        var tmdbResults = await _tMDbClient.SearchMovieAsync(gptMovie.Title, primaryReleaseYear: gptMovie.Year);
        var tmdbDetails = await _tMDbClient.GetMovieAsync(tmdbResults.Results.First().Id);

        return tmdbDetails?.ToTmdbMovie();
    }
}
